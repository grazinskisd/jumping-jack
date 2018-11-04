using DG.Tweening;
using UnityEngine;

namespace JumpingJack
{
    public class JumpState : PlayerState
    {
        private float _duration;

        public JumpState(Player player, PlayerStateMachine states)
            : base(player, states) { }

        public override void Enter()
        {
            Debug.Log("Enter Jump state");
            _player.Trigger(Triggers.Jump);
            SetDuration();
            IncrementHeightIndex();
            MovePlayerVertically();
            base.Enter();
        }

        private void SetDuration()
        {
            var animator = _player.Animator.runtimeAnimatorController;
            foreach(var anim in animator.animationClips)
            {
                if (anim.name.Equals("Jump"))
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
                .OnComplete(() => Exit(_states.Stand));
        }

        private void IncrementHeightIndex()
        {
            _player.PreviousHeightIndex = _player.CurrentHeightIndex;
            _player.CurrentHeightIndex = Mathf.Min(_player.CurrentHeightIndex + 1, _player.Settings.Heights.Positions.Length - 1);
        }
    }
}
