using UnityEngine;
using System.Collections;

namespace BogGames.Gameplay
{
    public class ExponentialEventTimer : MonoBehaviour
    {
        [Tooltip("Initial wait time in seconds")]
        public float startDelay = 10f;

        [Tooltip("Factor to multiply delay by each iteration (e.g. 0.9 = 10% shorter each time)")]
        public float decayFactor = 0.5f;

        [Tooltip("Minimum delay allowed (to prevent going to 0 or negative)")]
        public float minDelay = 2f;

        [Tooltip("The event to fire when the timer has run out")]
        public UnityEngine.Events.UnityEvent timerEvent;

        public void StartTimer()
        {
            StartCoroutine(TimerRoutine());
        }

        private IEnumerator TimerRoutine()
        {
            float currentDelay = startDelay;

            while (true)
            {
                yield return new WaitForSeconds(currentDelay);

                FireEvent();

                // Additive decay: decrease by fixed amount
                currentDelay = Mathf.Max(minDelay, currentDelay - decayFactor);
            }
        }

        private void FireEvent()
        {
            timerEvent?.Invoke();
        }
    }
}
