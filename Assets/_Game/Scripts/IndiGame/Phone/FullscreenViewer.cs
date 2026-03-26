using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

namespace IndiGame.Phone
{
    /// <summary>
    /// Visor a pantalla completa para las fotos de la galería.
    /// Zoom con scroll, cierre con Escape o click fuera.
    /// </summary>
    public class FullscreenViewer : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image photoImage;
        [SerializeField] private TextMeshProUGUI captionText;
        [SerializeField] private TextMeshProUGUI timestampText;
        [SerializeField] private RectTransform photoContainer;
        [SerializeField] private Button closeButton;

        [Header("Zoom")]
        [SerializeField] private float minZoom = 1f;
        [SerializeField] private float maxZoom = 3f;

        private float currentZoom = 1f;
        private bool isOpen;

        // ─────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────

        private void Awake()
        {
            gameObject.SetActive(false);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        private void Update()
        {
            if (!isOpen) return;

            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Close();
                return;
            }

            HandleZoom();
        }

        // ─────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────

        public void Open(PhotoData data)
        {
            if (data == null) return;

            gameObject.SetActive(true);
            currentZoom = 1f;
            if (photoContainer != null) photoContainer.localScale = Vector3.one;

            if (photoImage != null)
            {
                photoImage.sprite = data.photo;
                photoImage.color = data.photo != null ? Color.white : new Color(0.05f, 0.05f, 0.1f, 1f);
            }

            if (captionText != null)
            {
                captionText.text = data.caption;
                captionText.fontStyle = FontStyles.Italic;
            }

            if (timestampText != null)
                timestampText.text = data.timestamp;

            isOpen = true;
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }

        public void Close()
        {
            isOpen = false;
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }

        // ─────────────────────────────────────────────
        //  Zoom
        // ─────────────────────────────────────────────

        private void HandleZoom()
        {
            if (Mouse.current == null || photoContainer == null) return;

            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) < 0.01f) return;

            // scroll es ~120 por notch en Windows; normalizar a ~0.15 por notch
            float delta = scroll * 0.00125f;
            currentZoom = Mathf.Clamp(currentZoom + delta, minZoom, maxZoom);
            photoContainer.localScale = Vector3.one * currentZoom;
        }

        // ─────────────────────────────────────────────
        //  Fade
        // ─────────────────────────────────────────────

        private IEnumerator FadeIn()
        {
            if (canvasGroup == null) yield break;

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            float t = 0f;
            while (t < 0.15f)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / 0.15f);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOut()
        {
            if (canvasGroup == null)
            {
                gameObject.SetActive(false);
                yield break;
            }

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            float start = canvasGroup.alpha;
            float t = 0f;
            while (t < 0.15f)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(start, 0f, t / 0.15f);
                yield return null;
            }
            canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }
    }
}
