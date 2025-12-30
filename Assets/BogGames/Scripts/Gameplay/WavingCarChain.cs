using UnityEngine;
using System.Linq;

namespace BogGames.Gameplay
{
    public class WavingCarChain : MovingCar
    {
        [SerializeField] private bool includeInactive = false;

        private WavingCar[] wavingCarChain;

        public WavingCar[] WavingCarChainArray => wavingCarChain;

        protected void OnEnable()
        {
            wavingCarChain = GetComponentsInChildren<WavingCar>(includeInactive)
                                .Where(c => c.gameObject != gameObject)
                                .ToArray();
        }
    }
}
