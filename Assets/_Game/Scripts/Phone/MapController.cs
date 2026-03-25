using UnityEngine;
using UnityEngine.UI;
using IndiGame.Core;
using TMPro;
using System.Collections.Generic;

namespace IndiGame.Phone
{
    public class MapController : MonoBehaviour
    {
        [Header("References")]
        public RectTransform mapPanel;
        public RectTransform playerMarker;
        public RectTransform destinationMarker;
        public LineRenderer routeLine;
        
        [Header("UI Panels")]
        public TextMeshProUGUI addressText;
        public TextMeshProUGUI etaText;
        public CanvasGroup destinationMarkerGroup;

        [Header("Settings")]
        public float mapScale = 10f; // 1 unidad mundo = 10px mapa
        public Vector2 worldBounds = new Vector2(200, 200);

        private Transform playerTransform;
        private Vector3 currentDestination;
        private bool hasDestination;

        private void Start()
        {
            // Buscar al player si no está asignado
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null) playerTransform = playerObj.transform;

            // Suscribirse a eventos
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnDeliveryComplete += HandleDeliveryComplete;
            }
            
            // Inicialmente ocultar destino si no hay
            if (!hasDestination) HideDestination();
        }

        private void OnDestroy()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.OnDeliveryComplete -= HandleDeliveryComplete;
            }
        }

        private void Update()
        {
            if (playerTransform != null)
            {
                SetPlayerPosition(playerTransform.position);
            }

            if (hasDestination)
            {
                UpdateRouteLine();
            }
        }

        /// <summary>
        /// Configura el destino de la entrega en el mapa.
        /// </summary>
        public void SetDestination(Vector3 worldPos, string address)
        {
            currentDestination = worldPos;
            hasDestination = true;
            
            if (addressText != null) addressText.text = address;
            
            // Mostrar marcador
            if (destinationMarkerGroup != null) destinationMarkerGroup.alpha = 1f;
            
            // Posicionar marcador de destino
            Vector2 mapPos = WorldToMapCoords(worldPos);
            destinationMarker.anchoredPosition = mapPos;
            
            // Si tiene componente MapMarker, actualizar su posición base para la animación
            var marker = destinationMarker.GetComponent<MapMarker>();
            if (marker != null) marker.SetBasePosition(mapPos);
        }

        /// <summary>
        /// Actualiza la posición del icono del jugador en el mapa.
        /// </summary>
        public void SetPlayerPosition(Vector3 worldPos)
        {
            playerMarker.anchoredPosition = WorldToMapCoords(worldPos);
        }

        private void UpdateRouteLine()
        {
            if (routeLine == null) return;

            routeLine.positionCount = 2;
            
            // Usar coordenadas de UI para el LineRenderer
            // El LineRenderer en UI suele ser complicado si no está en modo ScreenSpace-Camera.
            // Pero aquí el usuario pidió LineRenderer. Asumiremos que está en un espacio que funciona.
            
            Vector3 startPos = playerMarker.position;
            Vector3 endPos = destinationMarker.position;
            
            routeLine.SetPosition(0, startPos);
            routeLine.SetPosition(1, endPos);
        }

        private Vector2 WorldToMapCoords(Vector3 worldPos)
        {
            // Mapear de mundo a local del panel
            // El panel es de 200x200 unidades de mundo -> panel completo mapping.
            // 200 unidades mundo = Ancho del RectTransform.
            // Si el panel mide 500x500 px, 1 unidad = 2.5 px.
            // El usuario dijo: "default 1 unidad = 10px en mapa", "200x200 unidades mundo = panel completo"
            // Esto implica que el panel mide 2000x2000px? O que la escala es relativa.
            
            // Seguiré la instrucción: coord * mapScale
            return new Vector2(worldPos.x * mapScale, worldPos.z * mapScale);
        }

        private void HandleDeliveryComplete(int deliveryId)
        {
            HideDestinationWithFade();
        }

        private void HideDestination()
        {
            hasDestination = false;
            if (destinationMarkerGroup != null) destinationMarkerGroup.alpha = 0f;
            if (routeLine != null) routeLine.positionCount = 0;
            if (addressText != null) addressText.text = "SIN ENTREGA ACTIVA";
            if (etaText != null) etaText.text = "-- min";
        }

        private void HideDestinationWithFade()
        {
            // Simple fade out - en un proyecto real usaríamos DOTween o una Corrutina
            // Por brevedad, lo ocultamos inmediatamente o con una lógica simple.
            StartCoroutine(FadeOutDestination());
        }

        private System.Collections.IEnumerator FadeOutDestination()
        {
            if (destinationMarkerGroup == null)
            {
                HideDestination();
                yield break;
            }

            float duration = 1f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                destinationMarkerGroup.alpha = 1f - (elapsed / duration);
                yield return null;
            }
            HideDestination();
        }
    }
}
