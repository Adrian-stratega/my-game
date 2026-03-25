using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
    public class PhoneController : MonoBehaviour
    {
        [Header("State")]
        [SerializeField] private PhoneState currentState = PhoneState.PHONE_HIDDEN;
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
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Volume postProcessVolume;

        private Vignette vignette;
        private float fovLerpTimer = 0f;
        private float inputTimer = 0f;
        private bool isLongPressing = false;

        // Posiciones del panel (Mockup)
        private Vector2 posHidden = new Vector2(0, -600f);
        private Vector2 posPocket = new Vector2(400f, -400f); // Esquina inferior derecha
        private Vector2 posActive = new Vector2(0, 0f);
        private Vector2 posFullscreen = new Vector2(0, 0f); // Se ajustará vía escala/ancho si es necesario

        private void Awake()
        {
            if (postProcessVolume != null && postProcessVolume.profile.TryGet(out vignette))
            {
                // Cache vignette
            }

            if (playerCamera == null) playerCamera = Camera.main;
            
            SetState(PhoneState.PHONE_HIDDEN, true);
        }

        private void Update()
        {
            HandleInput();
        }

        /// <summary>
        /// Maneja la entrada del usuario para el teléfono.
        /// </summary>
        private void HandleInput()
        {
            // Toggle POCKET <-> ACTIVE (F o Click Derecho)
            if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1))
            {
                inputTimer = 0f;
                isLongPressing = true;
            }

            if (isLongPressing)
            {
                inputTimer += Time.deltaTime;
                if (inputTimer >= longPressThreshold)
                {
                    // Long Press detectado: Toggle ACTIVE <-> FULLSCREEN
                    ToggleFullscreen();
                    isLongPressing = false;
                }
            }

            if ((Input.GetKeyUp(KeyCode.F) || Input.GetMouseButtonUp(1)) && isLongPressing)
            {
                // Click corto detectado: Toggle POCKET <-> ACTIVE
                TogglePocketActive();
                isLongPressing = false;
            }
        }

        /// <summary>
        /// Cambia entre los estados Pocket y Active.
        /// </summary>
        private void TogglePocketActive()
        {
            if (currentState == PhoneState.PHONE_POCKET || currentState == PhoneState.PHONE_HIDDEN)
                SetState(PhoneState.PHONE_ACTIVE);
            else if (currentState == PhoneState.PHONE_ACTIVE)
                SetState(PhoneState.PHONE_POCKET);
        }

        /// <summary>
        /// Cambia entre los estados Active y Fullscreen.
        /// </summary>
        private void ToggleFullscreen()
        {
            if (currentState == PhoneState.PHONE_ACTIVE)
                SetState(PhoneState.PHONE_FULLSCREEN);
            else if (currentState == PhoneState.PHONE_FULLSCREEN)
                SetState(PhoneState.PHONE_ACTIVE);
        }

        /// <summary>
        /// Cambia el estado del teléfono y dispara las transiciones correspondientes.
        /// </summary>
        public void SetState(PhoneState newState, bool immediate = false)
        {
            if (currentState == newState && !immediate) return;

            PhoneState oldState = currentState;
            currentState = newState;

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
            float targetWidthPercent = 0.35f;

            switch (state)
            {
                case PhoneState.PHONE_HIDDEN:
                    targetAlpha = 0f;
                    targetPos = posHidden;
                    break;
                case PhoneState.PHONE_POCKET:
                    targetAlpha = 0.5f; // Icono pequeño visible
                    targetPos = posPocket;
                    break;
                case PhoneState.PHONE_ACTIVE:
                    targetAlpha = 0.92f;
                    targetVignette = 0.25f;
                    targetFOV = activeFOV;
                    targetPos = posActive;
                    targetWidthPercent = 0.35f;
                    break;
                case PhoneState.PHONE_FULLSCREEN:
                    targetAlpha = 1.0f;
                    targetVignette = 0.45f;
                    targetFOV = activeFOV;
                    targetPos = posActive;
                    targetWidthPercent = 1.0f;
                    break;
            }

            float elapsed = 0f;
            float startAlpha = phoneCanvasGroup != null ? phoneCanvasGroup.alpha : 0f;
            float startVignette = vignette != null ? vignette.intensity.value : 0f;
            float startFOV = playerCamera != null ? playerCamera.fieldOfView : defaultFOV;
            Vector2 startPos = phonePanel != null ? phonePanel.anchoredPosition : posHidden;

            if (immediate)
            {
                elapsed = transitionDuration;
            }

            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / transitionDuration;
                float easedT = Mathf.SmoothStep(0, 1, t);

                if (phoneCanvasGroup != null) phoneCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, easedT);
                if (vignette != null) vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, easedT);
                if (playerCamera != null) playerCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, easedT);
                if (phonePanel != null) phonePanel.anchoredPosition = Vector2.Lerp(startPos, targetPos, easedT);

                yield return null;
            }

            // Asegurar valores finales
            if (phoneCanvasGroup != null) phoneCanvasGroup.alpha = targetAlpha;
            if (vignette != null) vignette.intensity.value = targetVignette;
            if (playerCamera != null) playerCamera.fieldOfView = targetFOV;
            if (phonePanel != null) phonePanel.anchoredPosition = targetPos;
            
            // Bloquear entrada del mundo en Fullscreen
            if (state == PhoneState.PHONE_FULLSCREEN)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        
        /// <summary>
        /// Reproduce el sonido de notificación.
        /// </summary>
        public void PlayNotificationSound()
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null && audio.clip != null)
            {
                audio.Play();
            }
        }
    }
}
