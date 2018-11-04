using DG.Tweening;
using UnityEngine;

namespace JumpingJack
{
    public class FallState : PlayerState
    {
        private const string FALL_ANIMATION = "Fall";
        private float _duration;

        public FallState(Player player, PlayerStateMachine states)
            :base(player, states) { }

        public override void Enter()
        {
            _player.Trigger(Triggers.Fall);
            SetDuration();
            DecrementHeightIndex();
            MovePlayerVertically();
            base.Enter();
        }

        private void SetDuration()
        {
            var animator = _player.Animator.runtimeAnimatorController;
            foreach (var anim in animator.animationClips)
            {
                if (anim.name.Equals(FALL_ANIMATION))
                {
                    _duration = anim.length / _player.GetFloat(Floats.FallSpeed);
                    break;
                }
            }
        }

        private void DecrementHeightIndex()
        {
            _player.PreviousHeightIndex = _player.CurrentHeightIndex;
            _player.CurrentHeightIndex = Mathf.Max(_player.CurrentHeightIndex - 1, 0);
        }

        private void MovePlayerVertically()
        {
            var desiredHeight = _player.Settings.Heights.Positions[_player.CurrentHeightIndex].y;
            _player.transform
                .DOLocalMoveY(desiredHeight, _duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Exit(_states.Stun));
        }
    }
}
