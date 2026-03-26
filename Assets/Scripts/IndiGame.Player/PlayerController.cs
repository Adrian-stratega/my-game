using UnityEngine;
using UnityEngine.InputSystem;

namespace IndiGame.Player
{
    /// <summary>
    /// Controlador principal del jugador. Maneja movimiento, cámara e interacciones básicas.
    /// Requerido por PhoneController.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Velocidad de movimiento base")]
        public float moveSpeed = 5f;
        [Tooltip("Multiplicador de velocidad al correr")]
        public float sprintMultiplier = 1.6f;
        [Tooltip("Fuerza de salto")]
        public float jumpHeight = 1.2f;
        [Tooltip("Gravedad aplicada")]
        public float gravity = -9.81f;

        [Header("Camera Settings")]
        [Tooltip("Transform de la cámara principal")]
        public Transform cameraTransform;
        [Tooltip("Sensibilidad del ratón")]
        public float mouseSensitivity = 100f;
        [Tooltip("Límite superior de rotación")]
        public float topClamp = 85f;
        [Tooltip("Límite inferior de rotación")]
        public float bottomClamp = -85f;

        [Header("Player State")]
        [Tooltip("Indica si el jugador tiene permitido moverse. Controlado externamente por sistemas como el Teléfono.")]
        public bool canMove = true;

        // Referencias privadas
        private CharacterController _characterController;
        private PlayerInput _playerInput;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private Vector3 _velocity;
        private bool _isSprinting;
        private float _xRotation;

        #region Unity Methods

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            
            // Si no se asigna cámara, intentar buscarla en los hijos
            if (cameraTransform == null)
            {
                var mainCam = GetComponentInChildren<Camera>();
                if (mainCam != null) cameraTransform = mainCam.transform;
            }
        }

        private void Start()
        {
            // Bloquear el cursor al inicio
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!canMove)
            {
                // Si no puede moverse, aseguramos que no haya velocidad residual horizontal
                _moveInput = Vector2.zero;
            }

            ApplyMovement();
            ApplyRotation();
        }

        #endregion

        #region Movement Logic

        /// <summary>
        /// Aplica el movimiento al Player basándose en el input y el estado.
        /// </summary>
        private void ApplyMovement()
        {
            // Movimiento horizontal
            float speed = moveSpeed * (_isSprinting ? sprintMultiplier : 1f);
            
            Vector3 targetDirection = transform.right * _moveInput.x + transform.forward * _moveInput.y;
            _characterController.Move(targetDirection * speed * Time.deltaTime);

            // Gravedad y Salto
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        /// <summary>
        /// Aplica la rotación de cámara y del cuerpo del jugador.
        /// </summary>
        private void ApplyRotation()
        {
            if (!canMove) return;

            float mouseX = _lookInput.x * mouseSensitivity * Time.deltaTime;
            float mouseY = _lookInput.y * mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, bottomClamp, topClamp);

            if (cameraTransform != null)
                cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }

        #endregion

        #region Input Handlers (PlayerInput SendMessages)

        public void OnMove(InputValue value)
        {
            if (!canMove) return;
            _moveInput = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            if (!canMove) return;
            _lookInput = value.Get<Vector2>();
        }

        public void OnSprint(InputValue value)
        {
            if (!canMove) return;
            _isSprinting = value.isPressed;
        }

        public void OnJump(InputValue value)
        {
            if (!canMove) return;
            if (value.isPressed && _characterController.isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        #endregion
    }
}
