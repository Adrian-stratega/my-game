using UnityEngine;

namespace IndiGame.Environment
{
    /// <summary>
    /// Hace parpadear una luz de forma ocasional e irregular para crear atmósfera de horror.
    /// </summary>
    public class FlickerLight : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Light targetLight;
        [SerializeField] private float baseIntensity = 3.0f;
        [SerializeField] private float flickerMinInterval = 8f;
        [SerializeField] private float flickerMaxInterval = 25f;
        [SerializeField] private float flickerDuration = 0.15f;

        private float _nextFlickerTime;
        #endregion

        #region Unity Methods
        private void Start()
        {
            if (targetLight == null) targetLight = GetComponent<Light>();
            ScheduleNextFlicker();
        }

        private void Update()
        {
            if (Time.time >= _nextFlickerTime)
                StartCoroutine(DoFlicker());
        }
        #endregion

        #region Private Methods
        private System.Collections.IEnumerator DoFlicker()
        {
            int flickerCount = Random.Range(1, 4);
            for (int i = 0; i < flickerCount; i++)
            {
                targetLight.intensity = 0f;
                yield return new WaitForSeconds(flickerDuration);
                targetLight.intensity = baseIntensity;
                yield return new WaitForSeconds(flickerDuration * 0.5f);
            }
            ScheduleNextFlicker();
        }

        private void ScheduleNextFlicker()
        {
            _nextFlickerTime = Time.time + Random.Range(flickerMinInterval, flickerMaxInterval);
        }
        #endregion
    }
}
