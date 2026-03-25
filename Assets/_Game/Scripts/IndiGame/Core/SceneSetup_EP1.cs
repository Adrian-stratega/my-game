using UnityEngine;
using IndiGame.Core;

namespace IndiGame.Core
{
    /// <summary>
    /// Inicializa el estado de la escena EP1 al entrar. 
    /// Debe ser el primer script en ejecutarse en la escena principal.
    /// </summary>
    public class SceneSetup_EP1 : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameManager gameManager;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (gameManager == null)
                gameManager = GameManager.Instance;

            if (gameManager != null)
            {
                gameManager.SetGameState(GameState.NORMALIDAD);
                Debug.Log("[SceneSetup_EP1] GameState inicializado: NORMALIDAD");
            }
            else
                Debug.LogError("[SceneSetup_EP1] GameManager no encontrado en la escena.");
        }
        #endregion
    }
}
