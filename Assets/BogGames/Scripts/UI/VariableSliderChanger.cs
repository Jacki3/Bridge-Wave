using UnityEngine;

namespace BogGames.UI
{
    public class VariableSliderChanger : MonoBehaviour
    {
        public UnityEngine.UI.Slider sliderToUpdate;
        public BogGames.Variables.BogFloatVariable floatVariable;

        public void SetMaxSliderValue()
        {
            if(sliderToUpdate && floatVariable)
            {
                sliderToUpdate.maxValue = floatVariable.Value;
            }
        }

        private void Update()
        {
            if(sliderToUpdate && floatVariable)
            {
                sliderToUpdate.value = floatVariable.Value;
            }
        }
    }
}
