namespace JumpingJack
{
    public delegate void PlayerStateEventHandler();
    public delegate void PlayerStateExitHandler(PlayerState nextState);

    public class PlayerState
    {
        public event PlayerStateEventHandler OnEnter;
        public event PlayerStateExitHandler OnExit;

        protected Player _player;
        protected PlayerStateMachine _states;

        public PlayerState(Player player, PlayerStateMachine states)
        {
            _player = player;
            _states = states;
        }

        public virtual void Enter()
        {
            IssueEvent(OnEnter);
        }

        public virtual void Update() { }

        public virtual void Exit(PlayerState nextState)
        {
            if(OnExit != null)
            {
                OnExit(nextState);
            }
        }

        protected void IssueEvent(PlayerStateEventHandler stateEvent)
        {
            if(stateEvent != null)
            {
                stateEvent();
            }
        }
    }
}
