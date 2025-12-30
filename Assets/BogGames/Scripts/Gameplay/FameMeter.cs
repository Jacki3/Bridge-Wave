using UnityEngine;

namespace BogGames.Gameplay
{
    public class FameMeter : CountdownMultiplier
    {
        [Header("Fame Meter Settings")]
        public MovingCarVariable activeCar;
        public float criticalTimeValue;
        [Header("Events")]
        public UnityEngine.Events.UnityEvent criticalTimeEvent;
        public UnityEngine.Events.UnityEvent criticalTimeEvadedEvent;

        private bool _inCritical;

        private void OnEnable()
        {
            _inCritical = IsNowCritical(timer);
        }

        private bool IsNowCritical(float t)
        {
            return t > 0f && t <= criticalTimeValue;
        }

        public override void AddToTimer(float value)
        {
            if(activeCar && activeCar.Value != null && sliderValueVar)
            {
                if(activeCar.Value is not WavingCar)
                    return;

                WavingCar wavingCar = activeCar.Value as WavingCar;

                timer += wavingCar.WaveMeterAdded;
                if(timer >= baseDuration)
                {
                    timer = baseDuration;
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            bool nowCritical = IsNowCritical(timer);

            if (nowCritical && !_inCritical)
            {
                _inCritical = true;
                criticalTimeEvent?.Invoke();
            }
            else if (!nowCritical && _inCritical)
            {
                _inCritical = false;
                criticalTimeEvadedEvent?.Invoke();
            }

        }
    }
}
