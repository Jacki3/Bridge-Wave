using UnityEngine;

namespace BogGames.Variables
{
    [CreateAssetMenu(fileName = "BogIntegerVariable", menuName = "Scriptable Objects/BogIntegerVariable")]
    public class BogIntegerVariable :  BogBaseVariable<int>
    {
        [SerializeField] protected int minValueAllowed;

        public override void Add(int value)
        {
            Value += value;
        }

        public override void Subtract(int value)
        {
            Value = Mathf.Max(Value - value, minValueAllowed);
        }
    }
}
