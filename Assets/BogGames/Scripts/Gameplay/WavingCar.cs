using UnityEngine;
using BogGames.Feedbacks;
using BogGames.Managers;

namespace BogGames.Gameplay
{
    public class WavingCar : MovingCar
    {
        [Range(0, 1)]
        [SerializeField] protected float rarityWeight;   
        [SerializeField] protected int score;
        [SerializeField] protected float waveMeterAdded;
        [SerializeField] protected MovingCarVariable activeCar;
        [SerializeField] protected FeedbackUserBase feedback;
        [SerializeField] protected Sprite carSprite;

        protected bool carResponded;

        public int Score => score;
        public float WaveMeterAdded => waveMeterAdded;
        public bool CarResponded => carResponded;
        public Sprite CarSprite => carSprite;
        public float CarRarity => rarityWeight;
        public Channel channel = Channel.Bridge;

        public void WaveBack()
        {
            if(activeCar == null || activeCar.Value == null)
                return;

            if(activeCar.Value.transform.position == transform.position)
            {
                feedback?.PlayFeedback();
                carResponded = true;
            }
        }    
    }
}