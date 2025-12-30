using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace BogGames.Managers
{
    public class SceneManager : MonoBehaviour
    {
        public void RestartCurrentScene()
        {
            UnitySceneManager.LoadScene(UnitySceneManager.GetActiveScene().buildIndex);
        }
    }
}