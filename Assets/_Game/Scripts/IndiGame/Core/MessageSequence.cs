using UnityEngine;

namespace IndiGame.Core
{
    /// <summary>
    /// Define una secuencia ordenada de mensajes.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMessageSequence", menuName = "IndiGame/Chat/MessageSequence")]
    public class MessageSequence : ScriptableObject
    {
        public MessageData[] messages;
    }
}