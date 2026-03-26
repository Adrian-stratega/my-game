using System;
using UnityEngine;
using UnityEngine.UI;

namespace IndiGame.Phone
{
    /// <summary>
    /// Controla el sistema de tabs del teléfono.
    /// Muestra/oculta paneles de contenido al cambiar de tab.
    /// </summary>
    public class PhoneTabController : MonoBehaviour
    {
        [Serializable]
        public struct TabEntry
        {
            public Button tabButton;
            public GameObject contentPanel;
            public Image tabIcon;
        }

        [Header("Tabs")]
        [SerializeField] private TabEntry[] tabs;
        [SerializeField] private int defaultTabIndex = 0;

        [Header("Colors")]
        [SerializeField] private Color activeColor   = new Color(1f, 0.42f, 0.21f);   // #FF6B35
        [SerializeField] private Color inactiveColor = new Color(0.94f, 0.94f, 0.94f, 0.5f); // #f0f0f0 50%

        private int currentIndex = -1;

        // ─────────────────────────────────────────────
        //  Lifecycle
        // ─────────────────────────────────────────────

        private void Awake()
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                int captured = i;
                if (tabs[i].tabButton != null)
                    tabs[i].tabButton.onClick.AddListener(() => ShowTab(captured));
            }
        }

        private void Start()
        {
            ShowTab(defaultTabIndex);
        }

        // ─────────────────────────────────────────────
        //  Public API
        // ─────────────────────────────────────────────

        public void ShowTab(int index)
        {
            if (index < 0 || index >= tabs.Length) return;
            if (currentIndex == index) return;

            currentIndex = index;

            for (int i = 0; i < tabs.Length; i++)
            {
                bool active = i == index;

                if (tabs[i].contentPanel != null)
                    tabs[i].contentPanel.SetActive(active);

                if (tabs[i].tabIcon != null)
                    tabs[i].tabIcon.color = active ? activeColor : inactiveColor;
            }
        }
    }
}
