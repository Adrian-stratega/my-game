using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace IndiGame.UI
{
    public class TransitionController : MonoBehaviour
    {
        public static TransitionController Instance { get; private set; }

        [SerializeField] private Image fadePanel;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start()
        {
            if (fadePanel != null)
            {
                fadePanel.color = new Color(0, 0, 0, 0);
                fadePanel.gameObject.SetActive(false);
            }
        }

        public IEnumerator FadeToBlack(float duration = 0.5f)
        {
            if (fadePanel == null) yield break;
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, Mathf.Clamp01(elapsed / duration));
                yield return null;
            }
            fadePanel.color = new Color(0, 0, 0, 1f);
        }

        public IEnumerator FadeFromBlack(float duration = 0.5f)
        {
            if (fadePanel == null) yield break;
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, 1f - Mathf.Clamp01(elapsed / duration));
                yield return null;
            }
            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel.gameObject.SetActive(false);
        }
    }
}
