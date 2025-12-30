using UnityEngine;

namespace BogGames.UI
{
    public class MovementButtonFlip : MonoBehaviour
    {
        public Transform target; 
        public float thresholdLessThan = 0f; 
        public float thresholdMoreThan = 0f; 
        public float rotationIfLess = 0f;   
        public float rotationIfGreater = 90f; 

        private void Update()
        {
            float valueToCheck = target.localPosition.z;

            if (valueToCheck < thresholdLessThan)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotationIfLess);
            }
            if(valueToCheck > thresholdMoreThan)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, rotationIfGreater);
            }
        }
    }
}
