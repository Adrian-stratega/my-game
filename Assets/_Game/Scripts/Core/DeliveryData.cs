using UnityEngine;
using IndiGame.Core;

namespace IndiGame.Core
{
    public enum DeliveryStatus
    {
        PENDING,
        ACTIVE,
        DELIVERED,
        FAILED
    }

    [CreateAssetMenu(fileName = "NewDelivery", menuName = "IndiGame/Delivery Data")]
    public class DeliveryData : ScriptableObject
    {
        public string clientName;
        public string address;
        public Vector3 worldDestination;
        public float estimatedTime; // en minutos
        public MessageSequence introSequence;
        public DeliveryStatus status = DeliveryStatus.PENDING;
    }
}
