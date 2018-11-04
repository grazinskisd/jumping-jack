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

            SetupStateListeners(_stateMachine.Stand, OnStand);
            SetupStateListeners(_stateMachine.Jump, OnJump);
            SetupStateListeners(_stateMachine.BadJump, OnBadJump);
            SetupStateListeners(_stateMachine.Move, OnMove);
            SetupStateListeners(_stateMachine.Fall, OnFall);
            SetupStateListeners(_stateMachine.Stun, OnStun);

            SwitchState(_stateMachine.Stand);
        }

        private void SetupStateListeners(PlayerState state, PlayerEventHanlder eventOnStart)
        {
            IssueEventOnStateEnter(state, eventOnStart);
            AddExitListener(state);
        }

        private void IssueEventOnStateEnter(PlayerState state, PlayerEventHanlder eventToIssue)
        {
            state.OnEnter += () => IssueEvent(eventToIssue);
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
            Debug.Log("Move trigger: " + trigger);
            Animator.SetTrigger(trigger.ToString());
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
}
