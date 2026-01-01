using UnityEngine;
using System.Collections;
using BogGames.Variables;

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

        [Tooltip("Maximum delay allowed (to prevent going to infinite)")]
        public float maxDelay = 10.0f;

        [Tooltip("If true this will increase the timer over time, rather than when false it will decrease more quickly.")]
        public bool increase = true;

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

                float change = increase ? decayFactor : -decayFactor;

                currentDelay = Mathf.Clamp(currentDelay + change, minDelay, maxDelay);            
            }
        }

        private void FireEvent()
        {
            timerEvent?.Invoke();
        }
    }
}
