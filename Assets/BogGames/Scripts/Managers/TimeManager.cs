using UnityEngine;

namespace BogGames.Managers
{
    public class TimeManager : MonoBehaviour
    {
        private float gameTime;
        private float timeScale = 1f;
        private bool isPaused;

        void Update()
        {
            if (isPaused) return;
            gameTime += Time.deltaTime * timeScale;
        }

        public void ResumeTime()
        {
            isPaused = false;
            Time.timeScale = timeScale;
        }

        public void StopTime()
        {
            isPaused = true;
            Time.timeScale = 0f;
        }

        public void ResetTime()
        {
            gameTime = 0f;
        }

        public float GetElapsedTime() => gameTime;
    }
}
