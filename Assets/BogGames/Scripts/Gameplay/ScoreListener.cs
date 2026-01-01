using UnityEngine;
using BogGames.Managers;

namespace BogGames.Gameplay
{
    public class ScoreListener : MonoBehaviour
    {
        [SerializeField] protected float maxMultiplier = 10;
        [SerializeField] protected MovingCarVariable activeCar;
        [SerializeField] protected BogGames.Variables.BogIntegerVariable scoreVariable;
        [SerializeField] protected BogGames.Variables.BogIntegerVariable activeCarScore;
        [SerializeField] protected BogGames.Variables.BogIntegerVariable multiplierVariable;
        [SerializeField] protected BogGames.Variables.BogIntegerVariable highScoreVariable;

        public void Start()
        {
            var scoreManager = BogSingleton.Instance.ScoreManager;
            if(highScoreVariable)
                highScoreVariable.Value = scoreManager.HighScore;
        }

        public void AddCarScore()
        {
            if(activeCar == null || activeCar.Value == null)
                return;

            if(activeCar.Value is not WavingCar)
                return;

            var wavingCar = activeCar.Value as WavingCar;

            var scoreManager = BogSingleton.Instance.ScoreManager;
            scoreManager?.AddPoints(wavingCar.Score);

            if(scoreVariable)
            {
                scoreVariable.Value = scoreManager.Score;
            }

            if(activeCarScore)
            {
                activeCarScore.Value = wavingCar.Score * scoreManager.Multiplier;
            }
        }

        public void IncreaseMultiplier(int amount)
        {
            var scoreManager = BogSingleton.Instance.ScoreManager;
            if(scoreManager.Multiplier + amount <= maxMultiplier)
                scoreManager?.IncreaseMultiplier(amount);
            if(multiplierVariable)
                multiplierVariable.Value = scoreManager.Multiplier;
        }

        public void ResetScore()
        {
            scoreVariable.Value = 0;
            multiplierVariable.Value = 1;
            var scoreManager = BogSingleton.Instance.ScoreManager;
            scoreManager?.ResetScore();
            scoreManager?.ResetMultiplier();
        }
    }
}
