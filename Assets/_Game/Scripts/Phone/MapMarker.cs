using UnityEngine;
using UnityEngine.UI;

namespace IndiGame.Phone
{
    public enum MarkerType
    {
        PLAYER,
        DESTINATION,
        ANOMALY
    }

    public class MapMarker : MonoBehaviour
    {
        [Header("Settings")]
        public MarkerType markerType;
        public float pulseAmplitude = 0.2f;
        public float pulseSpeed = 1f;

        private RectTransform rectTransform;
        private Vector3 initialScale;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            initialScale = transform.localScale;
        }

        private void Update()
        {
            if (markerType == MarkerType.PLAYER || markerType == MarkerType.DESTINATION)
            {
                ApplyAnimation();
            }
        }

        /// <summary>
        /// Aplica la animación de pulso o bounce según el tipo de marcador.
        /// </summary>
        private void ApplyAnimation()
        {
            if (markerType == MarkerType.PLAYER)
            {
                // Pulse 1 -> 1.2 -> 1
                float scale = 1f + (Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2) * pulseAmplitude * 0.5f + (pulseAmplitude * 0.5f));
                transform.localScale = initialScale * scale;
            }
            else if (markerType == MarkerType.DESTINATION)
            {
                // Bounce suave
                float bounce = Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed * Mathf.PI)) * pulseAmplitude;
                rectTransform.anchoredPosition += new Vector2(0, bounce); 
                // Wait, adding to position every frame is wrong. Should be absolute offset.
            }
        }
        
        // Fixing the bounce logic
        private Vector2 baseAnchoredPosition;
        
        public void SetBasePosition(Vector2 pos)
        {
            baseAnchoredPosition = pos;
            if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = pos;
        }

        private void LateUpdate()
        {
            if (markerType == MarkerType.DESTINATION)
            {
                float bounce = Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed * Mathf.PI)) * pulseAmplitude * 50f; // Scale it up for UI
                rectTransform.anchoredPosition = baseAnchoredPosition + new Vector2(0, bounce);
            }
        }
    }
}
