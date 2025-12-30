using UnityEngine;
using System.Collections.Generic;

namespace BogGames.Events
{
    [CreateAssetMenu(fileName = "ScriptableGameEvent", menuName = "Scriptable Objects/ScriptableGameEvent")]
    public class ScriptableGameEvent : ScriptableObject
    {
        private readonly List<ScriptableGameEventListener> listeners = new List<ScriptableGameEventListener>();

        public void Raise()
        {
            // Go backwards in case listeners get removed during iteration
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(ScriptableGameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(ScriptableGameEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }
}
