using UnityEngine;
using UnityEngine.InputSystem;
using IndiGame.Core;

namespace IndiGame.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("Velocidad de movimiento en m/s.")]
        public float walkSpeed = 2.5f;

        [Header("Look")]
        public Transform playerCamera;
        [Tooltip("Sensibilidad del ratón por defecto.")]
        public float mouseSensitivity = 2f;
        public float maxLookAngle = 85f;

        [Header("Head Bob")]
        public float bobAmplitude = 0.05f;
        public float bobFrequency = 1.5f;

        [HideInInspector] public bool canMove = true;

        private CharacterController characterController;
        private float cameraPitch = 0f;
        private float defaultCameraY;
        private float timer = 0f;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (playerCamera != null)
            {
                defaultCameraY = playerCamera.localPosition.y;
            }
            
            // Ocultar y bloquear el cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (!canMove) return;

            HandleMovement();
            HandleLook();
            HandleHeadBob();
        }

        /// <summary>
        /// Controla el movimiento del jugador usando el CharacterController.
        /// </summary>
        private void HandleMovement()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            float moveX = (keyboard.dKey.isPressed ? 1f : 0f) - (keyboard.aKey.isPressed ? 1f : 0f);
            float moveZ = (keyboard.wKey.isPressed ? 1f : 0f) - (keyboard.sKey.isPressed ? 1f : 0f);

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            
            // Gravedad simple
            if (!characterController.isGrounded)
            {
                move.y += Physics.gravity.y * Time.deltaTime;
            }

            characterController.Move(move * (walkSpeed * Time.deltaTime));
        }

        /// <summary>
        /// Controla la rotación de la cámara y del jugador.
        /// </summary>
        private void HandleLook()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            float mouseX = mouse.delta.x.ReadValue() * (mouseSensitivity * 0.1f);
            float mouseY = mouse.delta.y.ReadValue() * (mouseSensitivity * 0.1f);

            cameraPitch -= mouseY;
#if UNITY_EDITOR
            // Ajuste para el editor si el delta es muy alto
#endif
            cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

            if (playerCamera != null)
            {
                playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
            }
            
            transform.Rotate(Vector3.up * mouseX);
        }

        /// <summary>
        /// Aplica un efecto de balanceo a la cámara al caminar.
        /// </summary>
        private void HandleHeadBob()
        {
            if (playerCamera == null) return;

            float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
            
            if (speed > 0.1f && characterController.isGrounded)
            {
                timer += Time.deltaTime * bobFrequency;
                float newY = defaultCameraY + Mathf.Sin(timer * Mathf.PI * 2) * bobAmplitude;
                playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, newY, playerCamera.localPosition.z);
            }
            else
            {
                timer = 0;
                playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, Mathf.Lerp(playerCamera.localPosition.y, defaultCameraY, Time.deltaTime * 5f), playerCamera.localPosition.z);
            }
        }
    }
}
