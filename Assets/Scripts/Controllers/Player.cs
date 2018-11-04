using UnityEngine;

namespace JumpingJack
{
    public delegate void PlayerEventHanlder(Player sender);

    public class Player: MonoBehaviour
    {
        public PlayerSettings Settings;
        public Animator Animator;
        public PlayerAnimatorEvents AnimationEvents;
        public bool GodMode;

        public event PlayerEventHanlder OnJump;
        public event PlayerEventHanlder OnStun;
        public event PlayerEventHanlder OnMove;
        public event PlayerEventHanlder OnBadJump;
        public event PlayerEventHanlder OnHitHazard;
        public event PlayerEventHanlder OnStand;
        public event PlayerEventHanlder OnFall;

        public event PlayerEventHanlder OnGroundReached;
        public event PlayerEventHanlder OnTopReached;

        [HideInInspector]
        public int PreviousHeightIndex;
        [HideInInspector]
        public int CurrentHeightIndex;

        private bool _godMode;
        private PlayerPrefsService _prefService;

        private PlayerStateMachine _stateMachine;
        private PlayerState _state;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            SetupPlayerStateMachine();
            _godMode = (_prefService.GetInt(Prefs.GodMode) == 1) || GodMode;
            CurrentHeightIndex = 0;
            PreviousHeightIndex = 0;
        }

        private void SetupPlayerStateMachine()
        {
            _stateMachine = new PlayerStateMachine();
            _stateMachine.Stand = new StandState(this, _stateMachine);
            _stateMachine.Jump = new JumpState(this, _stateMachine);
            _stateMachine.BadJump = new BadJumpState(this, _stateMachine);
            _stateMachine.Move = new MoveState(this, _stateMachine);
            _stateMachine.Fall = new FallState(this, _stateMachine);
            _stateMachine.Stun = new StunState(this, _stateMachine);

            AddExitListener(_stateMachine.Stand);
            AddExitListener(_stateMachine.Jump);
            AddExitListener(_stateMachine.BadJump);
            AddExitListener(_stateMachine.Move);
            AddExitListener(_stateMachine.Fall);
            AddExitListener(_stateMachine.Stun);

            _stateMachine.Stand.OnEnter += () => IssueEvent(OnStand);
            _stateMachine.Jump.OnEnter += () => IssueEvent(OnJump);
            _stateMachine.BadJump.OnEnter += () => IssueEvent(OnBadJump);
            _stateMachine.Move.OnEnter += () => IssueEvent(OnMove);
            _stateMachine.Fall.OnEnter += () => IssueEvent(OnFall);
            _stateMachine.Stun.OnEnter += () => IssueEvent(OnStun);

            SwitchState(_stateMachine.Stand);
            Animator.ResetTrigger(Triggers.Stand.ToString());
        }

        private void AddExitListener(PlayerState state)
        {
            state.OnExit += SwitchState;
        }

        private void SwitchState(PlayerState newState)
        {
            _state = newState;
            _state.Enter();
        }

        private void Update()
        {
            _state.Update();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_godMode)
            {
                IssueEvent(OnHitHazard);
            }
        }

        public bool CanMoveUp()
        {
            if (HasHole(Vector2.up) || _godMode)
            {
                return true;
            }
            return false;
        }

        public bool ShouldFallDown()
        {
            if (HasHole(Vector2.down) && !_godMode)
            {
                return true;
            }
            return false;
        }

        public void Trigger(Triggers trigger)
        {
            Animator.SetTrigger(trigger.ToString());
        }

        public float GetFloat(Floats animatorFloat)
        {
            return Animator.GetFloat(animatorFloat.ToString());
        }

        private bool HasHole(Vector2 direction)
        {
            return Physics2D.Raycast(transform.position, direction, Settings.RayDistance, Settings.HoleLayerMask);
        }

        private void IssueEvent(PlayerEventHanlder eventToIssue)
        {
            if (eventToIssue != null)
            {
                eventToIssue(this);
            }
        }
    }

    public enum Triggers
    {
        Jump,
        Stand,
        BadJump,
        RunLeft,
        RunRight,
        Stun,
        Fall
    }

    public enum Floats
    {
        JumpSpeed,
        FallSpeed
    }
}
