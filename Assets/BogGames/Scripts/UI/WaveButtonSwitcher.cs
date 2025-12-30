using UnityEngine;

namespace BogGames.UI
{
    public class WaveButtonSwitcher : MonoBehaviour
    {
        public GameObject waveButtonLeft;
        public GameObject waveButtonRight;

        public virtual void ShowWaveButton()
        {
            if(!waveButtonLeft || !waveButtonRight)
                return;

            waveButtonLeft.SetActive(true);
            waveButtonRight.SetActive(true);
        }

        public virtual void HideWaveButton()
        {
            if(!waveButtonLeft || !waveButtonRight)
                return;

            waveButtonLeft.SetActive(false);
            waveButtonRight.SetActive(false);
        }
    }
}
