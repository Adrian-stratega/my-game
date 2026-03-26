using UnityEngine;
using UnityEngine.InputSystem;

namespace IndiGame.Phone
{
    /// <summary>
    /// TEST ONLY — eliminar antes del commit final de B6.
    /// R = bajar rating 0.5 | Y = subir rating 0.5
    /// N = notificación CRITICAL | M = notificación SUCCESS
    /// </summary>
    public class TestRatingTrigger : MonoBehaviour
    {
        private void Update()
        {
            var kb = Keyboard.current;
            if (kb == null) return;

            if (kb.rKey.wasPressedThisFrame)
                RatingController.Instance?.AddRating(-0.5f);

            if (kb.yKey.wasPressedThisFrame)
                RatingController.Instance?.AddRating(0.5f);

            if (kb.nKey.wasPressedThisFrame)
                AppNotificationController.Instance?.ShowNotification(
                    NotificationType.CRITICAL, "Marco. Sabemos que ya lo sabes. Completa el turno.");

            if (kb.mKey.wasPressedThisFrame)
                AppNotificationController.Instance?.ShowNotification(
                    NotificationType.SUCCESS, "¡Entrega completada! El cliente te dio 5 estrellas 🎉");
        }
    }
}
