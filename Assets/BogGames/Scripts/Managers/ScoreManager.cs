using UnityEngine;
using BogGames.Variables;

namespace BogGames.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Runtime")]
        [SerializeField] private int score = 0;
        [SerializeField] private BogIntegerVariable scoreVariable;
        
        private int highScore = 0;
        private int multiplier = 1;

        private const string HighScoreKey = "HighScore";

        public int Score => score;
        public int HighScore => highScore;
        public int Multiplier => multiplier;

        private void Awake()
        {
            highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        public void AddPoints(int amount)
        {
            if (amount == 0) return;

            int finalAmount = amount * multiplier;
            score = Mathf.Max(0, score + finalAmount);

            if (score > highScore)
            {
                highScore = score;
                PlayerPrefs.SetInt(HighScoreKey, highScore);
                PlayerPrefs.Save();
            }

            if(scoreVariable)
                scoreVariable.Value = score;
        }

        public void SetScore(int value)
        {
            score = Mathf.Max(0, value);

            if(scoreVariable)
                scoreVariable.Value = score;
        }

        public void ResetScore()
        {
            score = 0;

            if(scoreVariable)
                scoreVariable.Value = score;
        }

        public void ResetHighScore()
        {
            highScore = 0;
            PlayerPrefs.SetInt(HighScoreKey, 0);
            PlayerPrefs.Save();
        }

        public void IncreaseMultiplier(int amount)
        {
            multiplier = Mathf.Max(1, multiplier + amount); // Ensure it never goes below 1
        }

        public void ResetMultiplier()
        {
            multiplier = 1;
        }
    }
}
