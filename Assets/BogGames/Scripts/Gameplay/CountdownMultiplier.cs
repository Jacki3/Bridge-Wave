using UnityEngine;
using BogGames.Variables;
using BogGames.Helpers;
using UnityEngine.Events;

namespace BogGames.Gameplay
{
    public class CountdownMultiplier : MonoBehaviour
    {
        public float timer;
        protected bool timerStarted = false;

        public float baseDuration = 5f;
        [Tooltip("If true, will choose from a min and max value to define defaultWaitTime")]
        public bool useRandomWait;
        public BogFloatRange defaultWaitTimeRange;
        public BogFloatVariable defaultWaitTimeVar;
        public BogFloatVariable speedMultiplier;
        public BogFloatVariable sliderValueVar;
        public UnityEvent onTimeUp;

        public float CurrentTime => timer;

        protected virtual void Update()
        {
            if (timer > 0f)
            {
                timer -= Time.deltaTime * speedMultiplier.Value;
                timer = Mathf.Max(timer, 0f);

                // normalised progress (1 → 0 over the countdown)
                float t = timer / baseDuration;

                if(sliderValueVar)
                {
                    sliderValueVar.Value = CurrentTime;
                }
            }

            if(timer <= 0 && timerStarted)
                onTimeUp?.Invoke();
        }

        public virtual void StartTimer()
        {
            if(!useRandomWait)
                timer = baseDuration;
            else
            {
                timer = GetRandomWait();
            }

            if(defaultWaitTimeVar)
                defaultWaitTimeVar.Value = timer;

            timerStarted = true;
        }

        public virtual void StartTimer(BogFloatVariable value)
        {
            float defaultWait = baseDuration;
            if(useRandomWait)
                defaultWait = GetRandomWait();

            defaultWaitTimeVar.Value = defaultWait;

            timer = value.Value + defaultWait;
            timerStarted = true;
        }

        public virtual void AddToTimer(float value)
        {
            if(sliderValueVar)
                sliderValueVar.Value += value;

            if(timer >= baseDuration)
            {
                timer = baseDuration;
            }
        }

        public virtual void AddToTimer(BogFloatVariable value)
        {
            if(!value)
                return;

            float defaultWait = baseDuration;
            if(useRandomWait)
                defaultWait = GetRandomWait();

            defaultWaitTimeVar.Value = defaultWait;

            timer = value.Value + defaultWait;
        }

        private float GetRandomWait()
        {
            return defaultWaitTimeRange.Random;
        }
        
        private void OnDestroy()
        {
            if(sliderValueVar)
                sliderValueVar.Value = baseDuration;
        }
    }
}