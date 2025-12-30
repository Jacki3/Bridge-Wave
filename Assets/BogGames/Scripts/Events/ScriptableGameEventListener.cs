using UnityEngine;
using UnityEngine.Events;

namespace BogGames.Events
{
    public class ScriptableGameEventListener : MonoBehaviour
    {
        public ScriptableGameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            if (Event != null)
                Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (Event != null)
                Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response?.Invoke();
        }
    }
}
