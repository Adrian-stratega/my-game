using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using IndiGame.Core;
using System.Collections;

namespace IndiGame.Player
{
    /// <summary>
    /// Estados posibles del teléfono.
    /// </summary>
    public enum PhoneState
    {
        PHONE_HIDDEN,
        PHONE_POCKET,
        PHONE_ACTIVE,
        PHONE_FULLSCREEN
    }

    /// <summary>
    /// Controlador principal del sistema de teléfono/HUD.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PhoneController : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private PhoneState currentState = PhoneState.PHONE_HIDDEN;
        /// <summary>
        /// Obtiene el estado actual del teléfono.
        /// </summary>
        public PhoneState CurrentState => currentState;

        [Header("Animation Settings")]
        [SerializeField] private float transitionDuration = 0.25f;
        [SerializeField] private float longPressThreshold = 0.5f;

        [Header("FOV Settings")]
        [SerializeField] private float defaultFOV = 70f;
        [SerializeField] private float activeFOV = 56f;

        [Header("References")]
        [SerializeField] private CanvasGroup phoneCanvasGroup;
        [SerializeField] private RectTransform phonePanel;
        [SerializeField] private GameObject phoneCanvas;

        private PlayerController playerController;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Volume postProcessVolume;

        private Vignette vignette;
        private float inputTimer = 0f;
        private bool isLongPressing = false;

        // Posiciones del panel (Basadas en el diseño oficial B8)
        private Vector2 posHidden = new Vector2(0, -600f);
        private Vector2 posPocket = new Vector2(400f, -400f); 
        private Vector2 posActive = new Vector2(0, 0f);

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();

            if (postProcessVolume != null && postProcessVolume.profile != null && postProcessVolume.profile.TryGet(out vignette))
            {
                // Cache vignette
            }

            if (playerCamera == null) playerCamera = Camera.main;
            
            // Forzar ocultación inicial
            if (phoneCanvas != null) phoneCanvas.SetActive(false);
            SetState(PhoneState.PHONE_HIDDEN, true);
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;

            if (keyboard == null || mouse == null) return;

            // Toggle POCKET <-> ACTIVE (F o Click Derecho)
            if (keyboard.fKey.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
            {
                TogglePocketActive();
                inputTimer = 0f;
                isLongPressing = true;
            }

            if (isLongPressing && (keyboard.fKey.isPressed || mouse.rightButton.isPressed))
            {
                inputTimer += Time.deltaTime;
                if (inputTimer >= longPressThreshold)
                {
                    ToggleFullscreen();
                    isLongPressing = false;
                }
            }

            if (keyboard.fKey.wasReleasedThisFrame || mouse.rightButton.wasReleasedThisFrame)
            {
                isLongPressing = false;
            }
        }

        private void TogglePocketActive()
        {
            if (currentState == PhoneState.PHONE_POCKET || currentState == PhoneState.PHONE_HIDDEN)
                SetState(PhoneState.PHONE_ACTIVE);
            else if (currentState == PhoneState.PHONE_ACTIVE)
                SetState(PhoneState.PHONE_POCKET);
        }

        private void ToggleFullscreen()
        {
            if (currentState == PhoneState.PHONE_ACTIVE)
                SetState(PhoneState.PHONE_FULLSCREEN);
            else if (currentState == PhoneState.PHONE_FULLSCREEN)
                SetState(PhoneState.PHONE_ACTIVE);
        }

        /// <summary>
        /// Cambia el estado del teléfono y actualiza la visualización y el cursor.
        /// </summary>
        /// <param name="newState">El nuevo estado al que cambiar.</param>
        /// <param name="immediate">Si se debe aplicar el cambio sin animaciones.</param>
        public void SetState(PhoneState newState, bool immediate = false)
        {
            if (currentState == newState && !immediate) return;

            PhoneState oldState = currentState;
            currentState = newState;

            // Bloqueo de movimiento y cursor según GDD
            bool isInteracting = (newState == PhoneState.PHONE_ACTIVE || newState == PhoneState.PHONE_FULLSCREEN);
            
            if (playerController != null)
            {
                playerController.canMove = !isInteracting;
            }

            if (isInteracting)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // Activar canvas si el estado no es HIDDEN
            if (phoneCanvas != null)
            {
                phoneCanvas.SetActive(newState != PhoneState.PHONE_HIDDEN);
            }

            StopAllCoroutines();
            StartCoroutine(TransitionRoutine(newState, immediate));

            // Disparar eventos
            if (newState == PhoneState.PHONE_ACTIVE || newState == PhoneState.PHONE_FULLSCREEN)
            {
                if (oldState != PhoneState.PHONE_ACTIVE && oldState != PhoneState.PHONE_FULLSCREEN)
                    EventManager.Instance?.OnPhoneOpen?.Invoke();
            }
            else
            {
                if (oldState == PhoneState.PHONE_ACTIVE || oldState == PhoneState.PHONE_FULLSCREEN)
                    EventManager.Instance?.OnPhoneClose?.Invoke();
            }
        }

        private IEnumerator TransitionRoutine(PhoneState state, bool immediate)
        {
            float targetAlpha = 0f;
            float targetVignette = 0f;
            float targetFOV = defaultFOV;
            Vector2 targetPos = posHidden;

            switch (state)
            {
                case PhoneState.PHONE_HIDDEN:
                    targetAlpha = 0f;
                    targetPos = posHidden;
                    break;
                case PhoneState.PHONE_POCKET:
                    targetAlpha = 0.5f;
                    targetPos = posPocket;
                    break;
                case PhoneState.PHONE_ACTIVE:
                    targetAlpha = 0.92f;
                    targetVignette = 0.25f;
                    targetFOV = activeFOV;
                    targetPos = posActive;
                    break;
                case PhoneState.PHONE_FULLSCREEN:
                    targetAlpha = 1.0f;
                    targetVignette = 0.45f;
                    targetFOV = activeFOV;
                    targetPos = posActive;
                    break;
            }

            float elapsed = 0f;
            float startAlpha = phoneCanvasGroup != null ? phoneCanvasGroup.alpha : 0f;
            float startVignette = (vignette != null && vignette.intensity != null) ? vignette.intensity.value : 0f;
            float startFOV = playerCamera != null ? playerCamera.fieldOfView : defaultFOV;
            Vector2 startPos = phonePanel != null ? phonePanel.anchoredPosition : posHidden;

            if (immediate) elapsed = transitionDuration;

            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / transitionDuration;
                float easedT = Mathf.SmoothStep(0, 1, t);

                if (phoneCanvasGroup != null) phoneCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, easedT);
                if (vignette != null && vignette.intensity != null) vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, easedT);
                if (playerCamera != null) playerCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, easedT);
                if (phonePanel != null) phonePanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedT);

                yield return null;
            }

            if (phoneCanvasGroup != null) phoneCanvasGroup.alpha = targetAlpha;
            if (vignette != null && vignette.intensity != null) vignette.intensity.value = targetVignette;
            if (playerCamera != null) playerCamera.fieldOfView = targetFOV;
            if (phonePanel != null) phonePanel.anchoredPosition = targetPos;
            
            if (state == PhoneState.PHONE_HIDDEN && phoneCanvas != null)
                phoneCanvas.SetActive(false);
        }
        
        /// <summary>
        /// Reproduce el sonido de notificación si el AudioSource está configurado.
        /// </summary>
        public void PlayNotificationSound()
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null && audio.clip != null) audio.Play();
        }
    }
}
