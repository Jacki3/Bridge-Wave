using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BogGames.UI
{
    public class ControlsSwitcher : MonoBehaviour
    {
        [Header("Controls")]
        [Tooltip("The control to switch on/off")]
        public GameObject controlComponent;

        [Tooltip("The other control to switch on/off")]
        public GameObject otherControlComponent;

        [Tooltip("Start with the first control enabled? (If false, the other will start enabled)")]
        public bool startWithFirstControl = true;

        [Header("UI")]
        [Tooltip("The button that toggles between controls")]
        public Button toggleButton;

        [Tooltip("The label (Text) shown on the button")]
        public TextMeshProUGUI buttonLabel;

        [Tooltip("Button text when FIRST control is active")]
        public string textWhenFirstActive = "Switch to Other";

        [Tooltip("Button text when OTHER control is active")]
        public string textWhenOtherActive = "Switch to First";

        bool usingFirstControl;

        void Awake()
        {
            usingFirstControl = startWithFirstControl;
            ApplyControlState();
            UpdateButtonLabel();

            if (toggleButton != null)
                toggleButton.onClick.AddListener(OnToggleButtonPressed);
        }

        public void OnToggleButtonPressed()
        {
            usingFirstControl = !usingFirstControl;
            ApplyControlState();
            UpdateButtonLabel();
        }

        void ApplyControlState()
        {
            if (controlComponent != null)
                controlComponent.SetActive(usingFirstControl);

            if (otherControlComponent != null)
                otherControlComponent.SetActive(!usingFirstControl);
        }

        void UpdateButtonLabel()
        {
            if (buttonLabel == null) return;

            // When the FIRST control is currently active, show the text that hints switching to the OTHER one, and vice-versa
            buttonLabel.text = usingFirstControl ? textWhenFirstActive : textWhenOtherActive;
        }
    }
}
