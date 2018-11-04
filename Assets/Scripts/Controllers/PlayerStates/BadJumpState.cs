namespace JumpingJack
{
    public class BadJumpState : PlayerState
    {
        private float _duration;

        public BadJumpState(Player player, PlayerStateMachine states)
            : base(player, states) { }

        public override void Enter()
        {
            _player.Trigger(Triggers.BadJump);
            _player.AnimationEvents.OnBadJumpEnd += GoToStunState;
            base.Enter();
        }

        public override void Exit(PlayerState nextState)
        {
            _player.AnimationEvents.OnBadJumpEnd -= GoToStunState;
            base.Exit(nextState);
        }

        private void GoToStunState()
        {
            Exit(_states.Stun);
        }
    }
}
