using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IndiGame.UI
{
    /// <summary>
    /// Controla la pantalla de Game Over "Cuenta desactivada".
    /// Siempre activo en la escena; oculto mediante CanvasGroup alpha=0.
    /// </summary>
    public class GameOverController : MonoBehaviour
    {
        public static GameOverController Instance { get; private set; }

        [Header("Panel")]
        [SerializeField] private CanvasGroup gameOverPanel;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI logoText;
        [SerializeField] private TextMeshProUGUI mainMessage;
        [SerializeField] private TextMeshProUGUI subMessage;

        [Header("Buttons")]
        [SerializeField] private Button backToMenuButton;

        [Header("Settings")]
        [SerializeField] private float fadeDuration = 0.8f;
        [SerializeField] private float buttonDelay  = 3.0f;

        // ─────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            HideInstant();

            if (backToMenuButton != null)
                backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
        }

        // ─────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────

        public void ShowGameOver()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.interactable    = true;
                gameOverPanel.blocksRaycasts  = true;
            }
            StartCoroutine(GameOverSequence());
        }

        // ─────────────────────────────────────────────

        private void HideInstant()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.alpha           = 0f;
                gameOverPanel.interactable    = false;
                gameOverPanel.blocksRaycasts  = false;
            }
            if (backToMenuButton != null)
                backToMenuButton.gameObject.SetActive(false);
        }

        private IEnumerator GameOverSequence()
        {
            if (gameOverPanel == null) yield break;

            // Fade in
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                gameOverPanel.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                yield return null;
            }
            gameOverPanel.alpha = 1f;

            // Wait, then reveal button
            yield return new WaitForSeconds(buttonDelay);
            if (backToMenuButton != null)
                backToMenuButton.gameObject.SetActive(true);
        }

        private void OnBackToMenuClicked()
        {
            // B18 conectará esto al flujo de menú real.
            Debug.Log("[GameOverController] Volver al menú solicitado.");
            StartCoroutine(HideSequence());
        }

        private IEnumerator HideSequence()
        {
            if (backToMenuButton != null)
                backToMenuButton.gameObject.SetActive(false);

            float elapsed = 0f;
            while (elapsed < 0.4f)
            {
                elapsed += Time.deltaTime;
                if (gameOverPanel != null)
                    gameOverPanel.alpha = Mathf.Lerp(1f, 0f, elapsed / 0.4f);
                yield return null;
            }
            HideInstant();
        }
    }
}
