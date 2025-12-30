using UnityEngine;

namespace BogGames.Gameplay
{
    [CreateAssetMenu(fileName = "CarType", menuName = "Scriptable Objects/CarType")]
    public class CarType : ScriptableObject
    {
        public MovingCar prefab;
        public float minSpeed = 6f;
        public float maxSpeed = 12f;
        public float lengthMeters = 4f;
        public float rarityWeight = 1f;

        public float SampleSpeed(System.Random rng, float mult = 1f)
        {
            var t = (float)rng.NextDouble();
            return Mathf.Lerp(minSpeed, maxSpeed, t) * Mathf.Max(0.1f, mult);
        }    
    }
}
