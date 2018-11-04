using UnityEngine;

namespace JumpingJack
{
    public class StunState : PlayerState
    {
        private float _elapsedTime;

        public StunState(Player player, PlayerStateMachine states)
            : base(player, states) { }

        public override void Enter()
        {
            Debug.Log("Enter Stun state");
            _elapsedTime = 0.0f;
            _player.Trigger(Triggers.Stun);
            _player.OnHitHazard += GoToStunState;
            base.Enter();
        }

        public override void Update()
        {
            if (_player.ShouldFallDown())
            {
                Exit(_states.Fall);
            }
            else
            {
                _elapsedTime += Time.deltaTime;
                if(_elapsedTime >= _player.Settings.StunTime)
                {
                    Exit(_states.Stand);
                }
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
    }
}
