using UnityEngine;
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
            HandleMovement();
            HandleLook();
            HandleHeadBob();
        }

        /// <summary>
        /// Controla el movimiento del jugador usando el CharacterController.
        /// </summary>
        private void HandleMovement()
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

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
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            cameraPitch -= mouseY;
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
