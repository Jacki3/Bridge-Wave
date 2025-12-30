using UnityEngine;

namespace BogGames.Variables
{
    [CreateAssetMenu(fileName = "BogFloatVariable", menuName = "Scriptable Objects/BogFloatVariable")]
    public class BogFloatVariable : BogBaseVariable<float>
    {
        [SerializeField] protected float minValueAllowed;

        public override void Add(float value)
        {
            Value += value;
        }

        public override void Subtract(float value)
        {
            Value = Mathf.Max(Value - value, minValueAllowed);
        }
    }
}
