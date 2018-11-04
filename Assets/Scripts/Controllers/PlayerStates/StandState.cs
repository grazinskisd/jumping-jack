using UnityEngine;

namespace JumpingJack
{
    public class StandState : PlayerState
    {
        public StandState(Player player, PlayerStateMachine states)
            :base(player, states)
        {
        }

        public override void Enter()
        {
            Debug.Log("Enter Stand state");
            _player.Trigger(Triggers.Stand);
            _player.OnHitHazard += GoToStunState;
            base.Enter();
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
            else if (AreInputArrowsDown())
            {
                Exit(_states.Move);
            }
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

        private bool AreInputArrowsDown()
        {
            return Input.GetKey(_player.Settings.MoveLeft) || Input.GetKey(_player.Settings.MoveRight);
        }
    }
}
