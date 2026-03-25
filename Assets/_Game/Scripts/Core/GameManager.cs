using UnityEngine;

namespace IndiGame.Core
{
    public enum GameState
    {
        NORMALIDAD,
        ANOMALIA,
        INFILTRACION,
        REVELACION,
        ESCAPE,
        FINAL
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("State")]
        [SerializeField] private GameState currentState = GameState.NORMALIDAD;
        public GameState CurrentState => currentState;

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

        /// <summary>
        /// Cambia el estado actual del juego y dispara el evento correspondiente.
        /// </summary>
        public void SetGameState(GameState newState)
        {
            if (currentState == newState) return;
            
            currentState = newState;
            Debug.Log($"[GameManager] GameState cambiado a: {newState}");
            EventManager.Instance?.OnGameStateChanged?.Invoke(newState);
        }
    }
}
