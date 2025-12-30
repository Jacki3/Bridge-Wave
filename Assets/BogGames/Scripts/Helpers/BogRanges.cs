using UnityEngine;
using BogGames.Variables;

namespace BogGames.Helpers
{
    [System.Serializable]
    public class BogFloatRange
    {
        [SerializeField] private BogFloatVariable min;
        [SerializeField] private BogFloatVariable max;

        public float Min => min.Value;
        public float Max => max.Value;

        public float Random => UnityEngine.Random.Range(Min, Max);

        public BogFloatRange(float min, float max)
        {
            this.min = new BogFloatVariable();
            this.max = new BogFloatVariable();
            this.min.Value = min;
            this.max.Value = max;
        }

        public void Adjust(bool adjustMin, float amount)
        {
            if (adjustMin)
                min.Value += amount;
            else
                max.Value += amount;
        }

        public void ClampMin(float minValue)
        {
            if (Min < minValue)
                min.Value = minValue;
        }

        public void ClampMax(float maxValue)
        {
            if (Max > maxValue)
                max.Value = maxValue;
        }

        public bool Contains(float value)
        {
            return value >= Min && value <= Max;
        }

        public float Lerp(float t)
        {
            return Mathf.Lerp(Min, Max, t);
        }
    }

    [System.Serializable]
    public class BogIntegerRange
    {
        [SerializeField] private BogIntegerVariable min;
        [SerializeField] private BogIntegerVariable max;

        public int Min => min.Value;
        public int Max => max.Value;

        public int Random => UnityEngine.Random.Range(Min, Max);

        public BogIntegerRange(int min, int max)
        {
            this.min = new BogIntegerVariable();
            this.max = new BogIntegerVariable();
            this.min.Value = min;
            this.max.Value = max;
        }

        public void Adjust(bool adjustMin, int amount)
        {
            if (adjustMin)
                min.Value += amount;
            else
                max.Value += amount;
        }

        public void ClampMin(int minValue)
        {
            if (Min < minValue)
                min.Value = minValue;
        }

        public void ClampMax(int maxValue)
        {
            if (Max > maxValue)
                max.Value = maxValue;
        }

        public bool Contains(int value)
        {
            return value >= Min && value <= Max;
        }

        public float Lerp(float t)
        {
            return Mathf.Lerp(Min, Max, t);
        }
    }
}
