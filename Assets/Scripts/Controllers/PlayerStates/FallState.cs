using DG.Tweening;
using UnityEngine;

namespace JumpingJack
{
    public class FallState : PlayerState
    {
        private int _direction;
        private float _duration;

        public FallState(Player player, PlayerStateMachine states)
            :base(player, states) { }

        public override void Enter()
        {
            Debug.Log("Enter Fall state");
            _player.Trigger(Triggers.Fall);
            SetDuration();
            DecrementHeightIndex();
            MovePlayerVertically();
            base.Enter();
        }

        private void DecrementHeightIndex()
        {
            _player.PreviousHeightIndex = _player.CurrentHeightIndex;
            _player.CurrentHeightIndex = Mathf.Max(_player.CurrentHeightIndex - 1, 0);
        }

        private void SetDuration()
        {
            var animator = _player.Animator.runtimeAnimatorController;
            foreach (var anim in animator.animationClips)
            {
                if (anim.name.Equals("Fall"))
                {
                    _duration = anim.length;
                    break;
                }
            }
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
