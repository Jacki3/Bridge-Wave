using UnityEngine;
using UnityEngine.Events;

namespace BogGames.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] protected bool gamePaused = false;

        [SerializeField] protected UnityEvent globalStart;
        [SerializeField] protected UnityEvent gameStarted;

        private void Start() => globalStart?.Invoke();

        public void StartGame()
        {
            gameStarted.Invoke();
        }

        public void RestartGame()
        {
            BogSingleton.Instance.SceneManager.RestartCurrentScene();
        }

        public void PauseGame()
        {
            BogSingleton.Instance.TimeManager.StopTime();
            gamePaused = true;
        }

        public void ResumeGame()
        {
            BogSingleton.Instance.TimeManager.ResumeTime();
            gamePaused = false;
        }
    }
}
