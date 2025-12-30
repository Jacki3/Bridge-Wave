using UnityEngine;
using BogGames.Events;
using UnityEngine.Events;

namespace BogGames.Gameplay
{
    public class CollisionDetectionZone : MonoBehaviour
    {
        [Tooltip("Optional tag to check against")]
        public string gameTag;

        public GameObject ActiveObject { get; private set; }
        
        public UnityEvent objectHitCollider;
        public UnityEvent objectStaysCollider;
        public UnityEvent objectLeftCollider;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other.tag == gameTag)
            {
                objectHitCollider?.Invoke();
                
                ActiveObject = other.gameObject;
            }
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            bool tagMatches = true;
            if(!string.IsNullOrEmpty(gameTag))
            {
                tagMatches = false;
                if(other.tag == gameTag)
                {
                    tagMatches = true;
                }
            }
            if(tagMatches)
                ActiveObject = other.gameObject;
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if(other.tag != gameTag)
                return;
                
            if(other.gameObject == ActiveObject) ActiveObject = null;
            objectLeftCollider?.Invoke();
        }
    }
}