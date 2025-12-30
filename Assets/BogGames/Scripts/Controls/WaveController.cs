using UnityEngine;
using UnityEngine.Events;
using BogGames.Gameplay;

namespace BogGames.Controls
{
    public class WaveController : MonoBehaviour
    {
        [System.Serializable]
        protected class WavingPair
        {
            public string label = "Waving Pair";
            public CollisionDetectionZone bridgeDetector;
            public CollisionDetectionZone roadDetector;
        }

        private InputSystem_Actions controls;
        private MovingCar currentCar;

        public MovingCarVariable activeCar;
        [Header("Animation (optional)")]
        public Animator animator;
        public string waveTrigger = "PlayerWave";
        [Space]
        [Header("Events")]
        public UnityEvent waveEvent;

        private void OnEnable()  => controls.Player.Enable();
        private void OnDisable() => controls.Player.Disable();

        private void Awake()
        {
            controls = new InputSystem_Actions();

            controls.Player.Wave.performed += ctx => TryWave();
        }

        public void UpdateActiveCar()
        {
            if(activeCar != null && activeCar.Value != null)
            {
                currentCar = activeCar.Value;
            }
        }

        public void RemoveActiveCar()
        {
            currentCar = null;
        }

        public void TryWave()
        {
            if(currentCar != null)
            {
                if(animator && !string.IsNullOrEmpty(waveTrigger))
                    animator.SetTrigger(waveTrigger);

                waveEvent?.Invoke();
            }
        }
    }
}
