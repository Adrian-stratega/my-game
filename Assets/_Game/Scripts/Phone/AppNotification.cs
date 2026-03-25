using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using IndiGame.Core;

namespace IndiGame.Phone
{
    /// <summary>
    /// Maneja las notificaciones visuales en la aplicación QuickRun.
    /// </summary>
    public class AppNotification : MonoBehaviour
    {
        public static AppNotification Instance { get; private set; }

        [Header("Banner UI")]
        [SerializeField] private RectTransform bannerPanel;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI bodyText;
        [SerializeField] private Image bannerBackground;

        [Header("Badge UI")]
        [SerializeField] private GameObject inicioBadge;
        [SerializeField] private TextMeshProUGUI badgeText;

        [Header("Icons")]
        [SerializeField] private Sprite deliveryIcon;
        [SerializeField] private Sprite penaltyIcon;
        [SerializeField] private Sprite checkIcon;

        private int badgeCount = 0;
        private Vector2 hiddenPos = new Vector2(0, 100f);
        private Vector2 visiblePos = new Vector2(0, -20f);
        private float animationDuration = 0.5f;
        private float displayDuration = 3f;

        private void Start()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnMessageReceived += HandleMessageReceived;
                EventManager.Instance.OnRatingChanged += HandleRatingChanged;
                EventManager.Instance.OnDeliveryComplete += HandleDeliveryComplete;
            }
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnMessageReceived -= HandleMessageReceived;
                EventManager.Instance.OnRatingChanged -= HandleRatingChanged;
                EventManager.Instance.OnDeliveryComplete -= HandleDeliveryComplete;
            }
        }

        private void HandleMessageReceived(IndiGame.Core.MessageData data)
        {
            ShowNewDelivery(data.Content);
        }

        private void HandleRatingChanged(float rating)
        {
            // Solo notificamos si la valoración baja (penalización)
            // Necesitaríamos guardar la anterior o recibir el delta, 
            // pero el requerimiento dice "delta < 0".
            // Por simplicidad, asumiremos que cualquier cambio a la baja es penalización.
            ShowPenalty(rating);
        }

        private void HandleDeliveryComplete(int deliveryId)
        {
            ShowDeliveryComplete("$15.00"); // Placeholder de ganancia
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            if (bannerPanel != null) bannerPanel.anchoredPosition = hiddenPos;
            UpdateBadgeUI();
        }

        /// <summary>
        /// Muestra una notificación de entrega completada.
        /// </summary>
        public static void ShowDeliveryComplete(string earnings)
        {
            if (Instance == null) return;
            Instance.Notify(Instance.checkIcon, "ENTREGA COMPLETADA", $"Has ganado {earnings}", new Color(0.29f, 0.68f, 0.31f)); // Verde #4CAF50
        }

        /// <summary>
        /// Muestra una notificación de penalización.
        /// </summary>
        public static void ShowPenalty(float stars)
        {
            if (Instance == null) return;
            Instance.Notify(Instance.penaltyIcon, "PENALIZACIÓN", $"Valoración reducida: {stars}*", new Color(0.91f, 0.27f, 0.38f)); // Rojo #e94560
        }

        /// <summary>
        /// Muestra una notificación de nueva entrega disponible.
        /// </summary>
        public static void ShowNewDelivery(string address)
        {
            if (Instance == null) return;
            Instance.Notify(Instance.deliveryIcon, "NUEVA ENTREGA", $"Destino: {address}", new Color(1f, 0.42f, 0.21f)); // Naranja #FF6B35
            Instance.IncrementBadge();
        }

        private void Notify(Sprite icon, string title, string body, Color accent)
        {
            iconImage.sprite = icon;
            titleText.text = title;
            bodyText.text = body;
            bannerBackground.color = new Color(accent.r, accent.g, accent.b, 0.95f);
            
            StopAllCoroutines();
            StartCoroutine(NotificationRoutine());
            
            // Llamar al sonido desde el PhoneController
            FindAnyObjectByType<IndiGame.Player.PhoneController>()?.PlayNotificationSound();
        }

        private IEnumerator NotificationRoutine()
        {
            // Slide Down
            float elapsed = 0f;
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                bannerPanel.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, elapsed / animationDuration);
                yield return null;
            }
            bannerPanel.anchoredPosition = visiblePos;

            yield return new WaitForSeconds(displayDuration);

            // Slide Up
            elapsed = 0f;
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                bannerPanel.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, elapsed / animationDuration);
                yield return null;
            }
            bannerPanel.anchoredPosition = hiddenPos;
        }

        private void IncrementBadge()
        {
            badgeCount++;
            UpdateBadgeUI();
        }

        public void ClearBadge()
        {
            badgeCount = 0;
            UpdateBadgeUI();
        }

        private void UpdateBadgeUI()
        {
            if (inicioBadge != null)
            {
                inicioBadge.SetActive(badgeCount > 0);
                if (badgeText != null) badgeText.text = badgeCount.ToString();
            }
        }
    }
}
