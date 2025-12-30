using UnityEngine;

namespace BogGames.Managers
{
    [RequireComponent(typeof(SceneManager))]
    [RequireComponent(typeof(ScoreManager))]
    [RequireComponent(typeof(TimeManager))]
    public class BogSingleton : MonoBehaviour
    {
        // Singleton pattern that uses volatile to ensure that assignment to the instance variable completes before the instance variable can be accessed.
        volatile static BogSingleton instance;
        static bool applicationIsQuitting = false;
        // Singleton pattern that uses a private constructor to prevent instantiation of a class from other classes
        static readonly object padlock = new object();

        public SceneManager SceneManager { get; private set; }
        public ScoreManager ScoreManager { get; private set; }
        public TimeManager TimeManager { get; private set; }

        private void Awake()
        {
            SceneManager = GetComponent<SceneManager>();
            ScoreManager = GetComponent<ScoreManager>();
            TimeManager = GetComponent<TimeManager>();
        }

        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        /// it will create a buggy ghost object that will stay on the Editor scene
        /// even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        void OnDestroy()
        {
            applicationIsQuitting = true;
        }

        public static BogSingleton Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(BogSingleton) +
                        "' already destroyed on application quit." +
                        " Won't create again - returning null.");
                    return null;
                }
                // Double-checked locking pattern which (once the instance exists) avoids locking each time the method is invoked
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            GameObject bogSingleton = new GameObject("BogSingleton");
                            DontDestroyOnLoad(bogSingleton);
                            instance = bogSingleton.AddComponent<BogSingleton>();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
