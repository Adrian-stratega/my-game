using UnityEngine;

namespace IndiGame.Phone
{
    [CreateAssetMenu(fileName = "NewNotification", menuName = "LAST DELIVERY/Notification Data")]
    public class NotificationData : ScriptableObject
    {
        public NotificationType type;
        [TextArea(3, 5)]
        public string message;
        public float triggerDelay; // segundos después del evento
    }
}
