using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IndiGame.Core;
using IndiGame.UI;

namespace IndiGame.Phone
{
    /// <summary>
    /// Controla el sistema de Rating del jugador.
    /// Gestiona las estrellas UI, efectos visuales y el estado de Game Over.
    /// Singleton ligero — pertenece a la escena (no DontDestroyOnLoad).
    /// </summary>
    public class RatingController : MonoBehaviour
    {
        public static RatingController Instance { get; private set; }

        [Header("UI Elements")]
        [SerializeField] private Image[] stars;
        [SerializeField] private TextMeshProUGUI ratingText;
        [SerializeField] private Image phonePanelFlash;

        [Header("Settings")]
        [SerializeField] private float currentRating = 5.0f;

        private readonly Color fullStarColor  = new Color(1f,    0.42f, 0.21f, 1f);    // #FF6B35
        private readonly Color emptyStarColor = new Color(0.94f, 0.94f, 0.94f, 0.3f);  // #f0f0f0 30%
        private readonly Color redFlash       = new Color(0.91f, 0.27f, 0.38f, 0.45f); // #e94560
        private readonly Color greenFlash     = new Color(0.15f, 0.68f, 0.38f, 0.35f); // #27AE60

        private int lastIntRating;
        private Coroutine flashCoroutine;
        private Coroutine shakeCoroutine;

        // ─────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            lastIntRating = Mathf.FloorToInt(currentRating);
        }

        private void Start()
        {
            UpdateStarsUI();
        }

        // ─────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────

        public void SetRating(float newRating)
        {
            float oldRating = currentRating;
            currentRating = Mathf.Clamp(newRating, 0f, 5f);

            UpdateStarsUI();

            bool decreased = currentRating < oldRating;
            bool increased = currentRating > oldRating;

            if (decreased)
            {
                int affectedStar = Mathf.Max(0, Mathf.CeilToInt(currentRating));
                if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
                shakeCoroutine = StartCoroutine(ShakeStar(affectedStar));
                if (flashCoroutine != null) StopCoroutine(flashCoroutine);
                flashCoroutine = StartCoroutine(FlashEffect(redFlash, 0.2f));
            }
            else if (increased)
            {
                if (flashCoroutine != null) StopCoroutine(flashCoroutine);
                flashCoroutine = StartCoroutine(FlashEffect(greenFlash, 0.15f));
            }

            // Dispatch only when integer part changes
            int currentInt = Mathf.FloorToInt(currentRating);
            if (currentInt != lastIntRating)
            {
                lastIntRating = currentInt;
                EventManager.Instance?.OnRatingChanged?.Invoke(currentRating);
            }

            if (currentRating <= 0.0f)
                TriggerGameOver();
        }

        public void AddRating(float delta)
        {
            SetRating(currentRating + delta);
        }

        public float GetRating() => currentRating;

        // ─────────────────────────────────────────────
        //  Private helpers
        // ─────────────────────────────────────────────

        private void UpdateStarsUI()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                float threshold = i + 1f;
                if (currentRating >= threshold)
                {
                    stars[i].color = fullStarColor;
                    stars[i].fillAmount = 1f;
                }
                else if (currentRating > i)
                {
                    stars[i].color = fullStarColor;
                    stars[i].fillAmount = currentRating - i;
                }
                else
                {
                    stars[i].color = emptyStarColor;
                    stars[i].fillAmount = 1f;
                }
            }

            if (ratingText != null)
                ratingText.text = currentRating.ToString("F1");
        }

        private IEnumerator ShakeStar(int starIndex)
        {
            if (stars == null || starIndex >= stars.Length || starIndex < 0) yield break;

            Transform t = stars[starIndex].transform;
            Vector3 originalPos = t.localPosition;
            float duration = 0.25f;
            float elapsed = 0f;
            float intensity = 6f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                float shake = Mathf.Sin(progress * Mathf.PI * 8f) * intensity * (1f - progress);
                t.localPosition = originalPos + new Vector3(shake, shake * 0.5f, 0f);
                yield return null;
            }
            t.localPosition = originalPos;
        }

        private IEnumerator FlashEffect(Color color, float duration)
        {
            if (phonePanelFlash == null) yield break;

            phonePanelFlash.color = color;
            phonePanelFlash.gameObject.SetActive(true);

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Color c = color;
                c.a = Mathf.Lerp(color.a, 0f, elapsed / duration);
                phonePanelFlash.color = c;
                yield return null;
            }
            phonePanelFlash.gameObject.SetActive(false);
        }

        private void TriggerGameOver()
        {
            Debug.Log("[RatingController] GAME OVER: rating reached 0.");
            GameOverController.Instance?.ShowGameOver();
        }
    }
}
