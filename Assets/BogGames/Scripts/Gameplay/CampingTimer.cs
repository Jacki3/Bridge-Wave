using UnityEngine;
using BogGames.Variables;

namespace BogGames.Gameplay
{
    /// If player is sitting on one side for too long this will update a variable which can be used to increase timings or rarities elsewhere
    public class CampingTimer : MonoBehaviour
    {
        [Header("Variables")]
        [SerializeField] protected BogSideVariable playerSideVariable;
        [SerializeField] protected BogFloatVariable campingDampener;
        [SerializeField] protected BogFloatVariable campingRarityMultiplier;

        [Header("Settings")]
        [SerializeField] protected Side playerSide;
        [SerializeField] private float waitTime = 5.0f; // Delay before starting
    
        [Header("Fill Settings")]
        [SerializeField] private float targetValue = 5.0f;
        [SerializeField] private float timeToFill = 5.0f; 

        private float currentValue = 0f;
        private float currentTimer = 0f;

        private void Update()
        {
            if(!playerSideVariable || !campingDampener || !campingRarityMultiplier)
                return;

            if (playerSide == playerSideVariable.Value)
            {
                currentTimer += Time.deltaTime;

                if (currentTimer >= waitTime)
                {
                    float speed = targetValue / timeToFill;

                    currentValue = Mathf.MoveTowards(currentValue, targetValue, speed * Time.deltaTime);

                    campingDampener.Value = currentValue;
                    campingRarityMultiplier.Value = currentValue; // This likely needs to not be the same as timer
                }
            }
            else
            {
                ResetValues();
            }
        }

        private bool IsConditionMet()
        {
            return Input.GetKey(KeyCode.Space);
        }

        private void ResetValues()
        {
            currentTimer = 0f;
            currentValue = 0f;

            campingDampener.Value = currentValue;
            campingRarityMultiplier.Value = currentValue;
        }
    }   
}
