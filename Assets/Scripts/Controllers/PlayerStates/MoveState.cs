using DG.Tweening;
using UnityEngine;

namespace JumpingJack
{
    public class MoveState : PlayerState
    {
        private int _direction;
        private bool _isPlayerWrapping;

        public MoveState(Player player, PlayerStateMachine states)
            :base(player, states) { }

        public override void Enter()
        {
            base.Enter();
            _player.OnHitHazard += GoToStunState;
            SetDirection();
        }

        private void SetDirection()
        {
            _direction = 1;
            if (Input.GetKey(_player.Settings.MoveLeft))
            {
                _direction = -1;
            }
            var trigger = _direction == 1 ? Triggers.RunRight : Triggers.RunLeft;
            _player.Trigger(trigger);
        }

        public override void Update()
        {
            if (_player.ShouldFallDown())
            {
                Exit(_states.Fall);
            }
            else if (Input.GetKeyDown(_player.Settings.MoveUp))
            {
                Exit(_player.CanMoveUp() ? _states.Jump : _states.BadJump);
            }
            else if (AreInputArrowsUp())
            {
                Exit(_states.Stand);
            }
            else
            {
                MovePlayerHorizontaly();
                if (!_isPlayerWrapping && ShouldWrapPlayerPosition())
                {
                    SlidePlayerOut();
                }
            }
        }

        private void SlidePlayerOut()
        {
            Vector3 playerPos = _player.transform.position;
            float sign = playerPos.x / Mathf.Abs(playerPos.x);
            float dist = _player.Settings.RightSpawnPosition - _player.Settings.RightEndPosition;
            float dur = dist / _player.Settings.MoveSpeed;
            _isPlayerWrapping = true;

            _player.transform.DOMoveX(sign * _player.Settings.RightSpawnPosition, dur)
                .OnComplete(() => SlidePlayerIn(playerPos, sign, dur));
        }

        private void SlidePlayerIn(Vector3 playerPos, float sign, float dur)
        {
            playerPos.x = -sign * _player.Settings.RightSpawnPosition;
            _player.transform.position = playerPos;
            _player.transform.DOMoveX(-sign * _player.Settings.RightEndPosition, dur)
                .OnComplete(ResetIsPlayerWrapping);
        }

        private bool ShouldWrapPlayerPosition()
        {
            return Mathf.Abs(_player.transform.position.x) > _player.Settings.RightEndPosition;
        }

        private void ResetIsPlayerWrapping()
        {
            _isPlayerWrapping = false;
        }

        private void MovePlayerHorizontaly()
        {
            var playerPos = _player.transform.position;
            playerPos.x += _direction * _player.Settings.MoveSpeed * Time.deltaTime;
            _player.transform.position = playerPos;
        }

        public override void Exit(PlayerState nextState)
        {
            _player.OnHitHazard -= GoToStunState;
            base.Exit(nextState);
        }

        private void GoToStunState(Player sender)
        {
            Exit(_states.Stun);
        }

        private bool AreInputArrowsUp()
        {
            return Input.GetKeyUp(_player.Settings.MoveLeft) || Input.GetKeyUp(_player.Settings.MoveRight);
        }
    }
}
