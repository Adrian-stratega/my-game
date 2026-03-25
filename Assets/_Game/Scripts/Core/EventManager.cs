using System;
using UnityEngine;

namespace IndiGame.Core
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            // Debe ser root para que DontDestroyOnLoad funcione correctamente
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        // --- Core Events ---
        
        /// <summary>
        /// Se dispara al cambiar el estado del juego.
        /// </summary>
        public Action<GameState> OnGameStateChanged;

        // --- Phone/App Events ---
        
        /// <summary>
        /// Se dispara al abrir el teléfono.
        /// </summary>
        public Action OnPhoneOpen;

        /// <summary>
        /// Se dispara al cerrar el teléfono.
        /// </summary>
        public Action OnPhoneClose;

        /// <summary>
        /// Se dispara al recibir un nuevo mensaje.
        /// </summary>
        public Action<MessageData> OnMessageReceived;

        /// <summary>
        /// Se dispara al cambiar la puntuación (rating).
        /// </summary>
        public Action<float> OnRatingChanged;

        /// <summary>
        /// Se dispara al completar un envío. Envía el ID del envío.
        /// </summary>
        public Action<int> OnDeliveryComplete;

        /// <summary>
        /// Se dispara cuando aparece una foto en el chat de la secta.
        /// </summary>
        public Action<int> OnPhotoAppeared;

        // --- Doppelgänger Events ---

        /// <summary>
        /// Se dispara al activar el doppelgänger.
        /// </summary>
        public Action OnDoppelgangerActivate;
    }
}
