using DG.Tweening;
using UnityEngine;

namespace JumpingJack
{
    public class JumpState : PlayerState
    {
        private const string JUMP_ANIMATION = "Jump";

        private float _duration;

        public JumpState(Player player, PlayerStateMachine states)
            : base(player, states) { }

        public override void Enter()
        {
            _player.Trigger(Triggers.Jump);
            SetDuration();
            IncrementHeightIndex();
            MovePlayerVertically();
            base.Enter();
        }

        private void SetDuration()
        {
            var animator = _player.Animator.runtimeAnimatorController;
            foreach (var anim in animator.animationClips)
            {
                if (anim.name.Equals(JUMP_ANIMATION))
                {
                    _duration = anim.length / _player.GetFloat(Floats.JumpSpeed);
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
                .OnComplete(() => Exit(_states.Stand));
        }

        public override void Exit(PlayerState nextState)
        {
            base.Exit(TopFloor() ? _states.TopReached : nextState);
        }

        private bool TopFloor()
        {
            return _player.CurrentHeightIndex == _player.Settings.Heights.Positions.Length - 1;
        }

        private void IncrementHeightIndex()
        {
            _player.PreviousHeightIndex = _player.CurrentHeightIndex;
            _player.CurrentHeightIndex = Mathf.Min(_player.CurrentHeightIndex + 1, _player.Settings.Heights.Positions.Length - 1);
        }
    }
}
