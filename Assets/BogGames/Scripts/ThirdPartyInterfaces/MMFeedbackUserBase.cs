using UnityEngine;
using MoreMountains.Feedbacks;

namespace BogGames.ThirdPartyInterfaces
{
    [RequireComponent(typeof(MMF_Player))]
    public sealed class MMFFeedbackUser : BogGames.Feedbacks.FeedbackUserBase
    {
        [SerializeField] private MMF_Player _player;

        private void Awake()
        {
            if(_player == null)
                _player = GetComponent<MMF_Player>();
        }

        public override void PlayFeedback()
        {
            if (_player != null)
            {
                _player.PlayFeedbacks();
            }
        }
    }
}
