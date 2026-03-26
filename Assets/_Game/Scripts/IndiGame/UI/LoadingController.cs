using UnityEngine;
using TMPro;
using System.Collections;

namespace IndiGame.UI
{
    public class LoadingController : MonoBehaviour
    {
        public static LoadingController Instance { get; private set; }

        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI addressText;
        [SerializeField] private float charDelay = 0.05f;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Start()
        {
            if (loadingPanel != null) loadingPanel.SetActive(false);
        }

        public IEnumerator ShowLoading(string address, float totalDuration = 4f)
        {
            if (loadingPanel == null) yield break;
            loadingPanel.SetActive(true);
            if (addressText != null)
            {
                addressText.text = "";
                foreach (char c in address)
                {
                    addressText.text += c;
                    yield return new WaitForSeconds(charDelay);
                }
            }
            float remaining = totalDuration - (address.Length * charDelay);
            if (remaining > 0f) yield return new WaitForSeconds(remaining);
            loadingPanel.SetActive(false);
        }
    }
}
