using UnityEngine;

namespace BogGames.Cameras
{
    public class CameraTriggerArea : MonoBehaviour
    {
        public GameObject CameraToTurnOn;

        public void SwitchCamera(bool on)
        {
            if(CameraToTurnOn)
                CameraToTurnOn.SetActive(on);
        }
    }
}