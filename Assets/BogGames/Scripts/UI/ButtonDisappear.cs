using UnityEngine;
using UnityEngine.UI;

namespace BogGames.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonDisappear : MonoBehaviour
    {
        private Button button;

        void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            gameObject.SetActive(false);
        }
    }
}