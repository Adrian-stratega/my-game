using UnityEngine;

namespace IndiGame.Core
{
    /// <summary>
    /// Representa los datos de un mensaje individual en el sistema de chat.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMessageData", menuName = "IndiGame/Chat/MessageData")]
    public class MessageData : ScriptableObject
    {
        [Header("Content")]
        public string senderName = "QuickRun Support";
        [TextArea(3, 10)]
        public string messageText;
        
        [Header("Settings")]
        public float delayBeforeNext = 0.5f;
        public bool isPlayerResponse = false;
        public MessageTone tone = MessageTone.NORMAL;
    }
}
