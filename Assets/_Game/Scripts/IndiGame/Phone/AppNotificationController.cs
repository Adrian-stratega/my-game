using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IndiGame.Phone
{
    /// <summary>
    /// Controla el sistema de notificaciones de la app QuickRun.
    /// Banner de 56px en la parte superior del teléfono con cola de espera.
    /// </summary>
    public class AppNotificationController : MonoBehaviour
    {
        public static AppNotificationController Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private RectTransform bannerRoot;
        [SerializeField] private Image bannerBackground;
        [SerializeField] private TextMeshProUGUI iconText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private TextMeshProUGUI timestampText;
        [SerializeField] private Button dismissButton;

        [Header("Settings")]
        [SerializeField] private float visibleDuration = 3f;
        [SerializeField] private float slideDuration   = 0.25f;

        // Banner height = 56px; hiddenPos puts it fully above the phone top edge
        private readonly Vector2 hiddenPos  = new Vector2(0f, 56f);
        private readonly Vector2 visiblePos = new Vector2(0f, 0f);

        // Colores oficiales B8
        private readonly Color infoColor     = new Color(0.29f, 0.56f, 0.85f, 1f); // #4A90D9
        private readonly Color successColor  = new Color(0.15f, 0.68f, 0.38f, 1f); // #27AE60
        private readonly Color warningColor  = new Color(1f,    0.42f, 0.21f, 1f); // #FF6B35
        private readonly Color criticalColor = new Color(0.91f, 0.27f, 0.38f, 1f); // #e94560

        private readonly Queue<(NotificationType type, string message)> queue
            = new Queue<(NotificationType, string)>();

        private bool isDisplaying   = false;
        private bool criticalWaiting = false;
        private Coroutine pulseCoroutine;

        // ─────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            if (bannerRoot != null)
                bannerRoot.anchoredPosition = hiddenPos;

            if (dismissButton != null)
                dismissButton.onClick.AddListener(DismissCritical);
        }

        // ─────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────

        public void ShowNotification(NotificationType type, string message)
        {
            queue.Enqueue((type, message));
            if (!isDisplaying)
                StartCoroutine(ProcessQueue());
        }

        public void ShowNotification(NotificationData data)
        {
            if (data != null)
                ShowNotification(data.type, data.message);
        }

        // ─────────────────────────────────────────────
        //  Internal
        // ─────────────────────────────────────────────

        private IEnumerator ProcessQueue()
        {
            isDisplaying = true;

            while (queue.Count > 0)
            {
                var (type, msg) = queue.Dequeue();
                yield return DisplayNotification(type, msg);
            }

            isDisplaying = false;
        }

        private IEnumerator DisplayNotification(NotificationType type, string message)
        {
            SetupUI(type, message);

            // Slide down (hidden → visible)
            yield return AnimateBanner(hiddenPos, visiblePos, slideDuration, easeOut: true);

            if (type == NotificationType.CRITICAL)
            {
                criticalWaiting = true;
                pulseCoroutine = StartCoroutine(PulseEffect());

                // Wait until player dismisses
                while (criticalWaiting)
                    yield return null;

                if (pulseCoroutine != null)
                {
                    StopCoroutine(pulseCoroutine);
                    pulseCoroutine = null;
                }

                // Reset alpha
                if (bannerBackground != null)
                {
                    Color c = bannerBackground.color;
                    c.a = 1f;
                    bannerBackground.color = c;
                }
            }
            else
            {
                yield return new WaitForSeconds(visibleDuration);
            }

            // Slide up (visible → hidden)
            yield return AnimateBanner(visiblePos, hiddenPos, slideDuration * 0.8f, easeOut: false);
        }

        private void SetupUI(NotificationType type, string message)
        {
            if (messageText != null)  messageText.text  = message;
            if (timestampText != null) timestampText.text = System.DateTime.Now.ToString("HH:mm");

            Color bgColor  = infoColor;
            string icon    = "i";

            switch (type)
            {
                case NotificationType.SUCCESS:
                    bgColor = successColor;
                    icon    = "✓";
                    break;
                case NotificationType.WARNING:
                    bgColor = warningColor;
                    icon    = "!";
                    break;
                case NotificationType.CRITICAL:
                    bgColor = criticalColor;
                    icon    = "!!";
                    break;
            }

            if (bannerBackground != null) bannerBackground.color = bgColor;
            if (iconText != null)         iconText.text = icon;
        }

        private IEnumerator AnimateBanner(Vector2 from, Vector2 to, float duration, bool easeOut)
        {
            if (bannerRoot == null) yield break;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = easeOut
                    ? 1f - Mathf.Pow(1f - t, 3f)  // ease-out cubic
                    : t * t;                        // ease-in quad
                bannerRoot.anchoredPosition = Vector2.Lerp(from, to, eased);
                yield return null;
            }
            bannerRoot.anchoredPosition = to;
        }

        private IEnumerator PulseEffect()
        {
            while (criticalWaiting)
            {
                float alpha = Mathf.Lerp(1.0f, 0.6f, Mathf.PingPong(Time.time * 2f, 1f));
                if (bannerBackground != null)
                {
                    Color c = bannerBackground.color;
                    c.a = alpha;
                    bannerBackground.color = c;
                }
                yield return null;
            }
        }

        private void DismissCritical()
        {
            criticalWaiting = false;
        }
    }
}
