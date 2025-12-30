using UnityEngine;

namespace BogGames.Gameplay
{
    public class MovingCar : MonoBehaviour
    {
        public float speed = 8f;
        public Transform exitTarget;     // set by spawner node
        public float maxLifetime = 15f;

        float _t;

        void Update()
        {
            float dt = Time.deltaTime;
            _t += dt;
            transform.position += transform.forward * speed * dt;

            bool lifetimeUp = _t >= maxLifetime;
            bool passedExit = exitTarget && Vector3.Dot(transform.forward, exitTarget.position - transform.position) <= 0f;

            if (lifetimeUp || passedExit)
                Destroy(gameObject);
        }
    }
}