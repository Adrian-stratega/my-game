using UnityEngine;

namespace IndiGame.Core
{
    public enum Language
    {
        EN,
        ES
    }

    [CreateAssetMenu(fileName = "GameSettings", menuName = "IndiGame/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Controls")]
        [Tooltip("Sensibilidad visual (Mouse/Gamepad).")]
        public float sensitivity = 2.0f;

        [Header("Audio")]
        [Range(0f, 1f)]
        [Tooltip("Volumen maestro del juego.")]
        public float volume = 1.0f;

        [Header("Localization")]
        [Tooltip("Idioma actual del juego.")]
        public Language language = Language.ES;
    }
}