using UnityEngine;
using UnityEngine.Events;

namespace BogGames.Gameplay
{
    public class RoadDetectionZone : CollisionDetectionZone
    {
        [Tooltip("The object that is paired to this one")]
        public CollisionDetectionZone pairedCollisionZone;
        public MovingCarVariable activeCar;
        public MovingCarVariable disabledCar;

        protected override void OnTriggerStay(Collider other)
        {
            if(pairedCollisionZone.ActiveObject != null)
            {
                base.OnTriggerStay(other);

                if(ActiveObject != null)
                {
                    WavingCar thisCar = ActiveObject.GetComponent<WavingCar>();

                    if(thisCar != null && activeCar != null && thisCar.CarResponded == false)
                    {
                        activeCar.Value = thisCar;
                        objectStaysCollider?.Invoke();
                    }
                }
            }
        }

        protected override void OnTriggerExit(Collider other)
        {
            if(ActiveObject != null)
            {
                WavingCar thisCar = ActiveObject.GetComponent<WavingCar>();

                if(thisCar != null && activeCar != null && activeCar.Value != null)
                {
                    activeCar.Value = null;
                }
                disabledCar.Value = thisCar;
            }
            base.OnTriggerExit(other);
        }
    }
}
