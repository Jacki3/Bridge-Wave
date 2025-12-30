using UnityEngine;
using TMPro;
using System.Collections;

namespace BogGames.UI
{
    public class VariableTMPTextChanger : MonoBehaviour
    {
        public TextMeshProUGUI target;
        public BogGames.Variables.BogVariable variable;

        [Tooltip("If using a float variable you can 'count' up from current text value to the value of the float variable. This is how long the animation lasts.")]
        public float duration = 0.5f;

        private Coroutine countRoutine;

        public void SetTextFromVariable()
        {
            if(target == null || variable == null || variable.GetValue() == null)
                return;

            var value = variable.GetValue();
            string variableAsString = value.ToString();

            target.text = variableAsString;
        }

        public void SetTextFromVariableWithModifier(string modifier)
        {
            if(target == null || variable == null || variable.GetValue() == null)
                return;

            var value = variable.GetValue();
            string variableAsString = value.ToString();

            target.text = modifier + variableAsString;
        }

        public void UpdateNumber(BogGames.Variables.BogIntegerVariable newValue)
        {
            if(newValue == null)
                return;
                    
            if (countRoutine != null)
                StopCoroutine(countRoutine);

            countRoutine = StartCoroutine(CountTo(newValue.Value));
        }

        private IEnumerator CountTo(int numericalTarget)
        {
            int.TryParse(target.text, out int startValue);

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, numericalTarget, t));
                target.text = currentValue.ToString();

                yield return null;
            }

            target.text = numericalTarget.ToString();
        }
    }
}
