using UnityEngine;
using DG.Tweening;

namespace JumpingJack
{
    //public delegate void PlayerEventHanlder(PlayerController sender);

    public class PlayerController : MonoBehaviour
    {
        public PlayerSettings Settings;
        public bool GodMode;

        public event PlayerEventHanlder OnJump;
        public event PlayerEventHanlder OnStun;
        public event PlayerEventHanlder OnRunLeft;
        public event PlayerEventHanlder OnRunRight;
        public event PlayerEventHanlder OnBadJump;
        public event PlayerEventHanlder OnHitHazard;
        public event PlayerEventHanlder OnEndCurrent;
        public event PlayerEventHanlder OnFall;

        public event PlayerEventHanlder OnGroundReached;
        public event PlayerEventHanlder OnTopReached;

        private int _previousHeightIndex;
        private int _currentHeightIndex;
        private bool _isMovingVertically;

        private bool _isStunned;
        private float _timeSinceStunned;

        private bool _godMode;

        private PlayerPrefsService _prefService;

        private bool _isPlayerWrapping;
        private bool _isMovingHorizontally;
        private int _direction;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            _godMode = (_prefService.GetInt(Prefs.GodMode) == 1) || GodMode;
            _currentHeightIndex = 0;
            _previousHeightIndex = 0;
            _isMovingVertically = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_godMode)
            {
                Stun();
                IssueEvent(OnHitHazard);
            }
        }

        private void Stun()
        {
            _isStunned = true;
            _timeSinceStunned = 0;
            IssueEvent(OnStun);
        }

        private void Update()
        {
            if (_isStunned)
            {
                UpdateStunTime();
            }

            if (!_isMovingVertically)
            {
                ProcessUserInput();
            }
        }

        private void ResetIsPlayerWrapping()
        {
            _isPlayerWrapping = false;
        }

        private void ProcessUserInput()
        {
            ProcessVerticalInput();
            ProcessHorizontalInput();
        }

        private void ProcessVerticalInput()
        {
            if (Input.GetKeyDown(Settings.MoveUp) && !_isStunned)
            {
                if (CanMoveUp())
                {
                    IncrementHeightIndex();
                    MovePlayerVertically();
                    IssueEvent(OnJump);
                }
                else
                {
                    CheckForEndConditions();
                    Stun();
                    IssueEvent(OnBadJump);
                }
            }
            else if (ShouldFallDown())
            {
                DecrementHeightIndex();
                MovePlayerVertically();
                IssueEvent(OnFall);
            }
        }

        private void ProcessHorizontalInput()
        {
            if (AreInputArrowsUp())
            {
                _isMovingHorizontally = false;
            }

            if (!_isStunned)
            {
                if (AreInputArrowsUp())
                {
                    IssueEvent(OnEndCurrent);
                    _isMovingHorizontally = false;
                }

                if (Input.GetKeyDown(Settings.MoveLeft))
                {
                    _isMovingHorizontally = true;
                    _direction = -1;
                    IssueEvent(OnRunLeft);
                }
                else if (Input.GetKeyDown(Settings.MoveRight))
                {
                    _isMovingHorizontally = true;
                    _direction = 1;
                    IssueEvent(OnRunRight);
                }

                if (_isMovingHorizontally)
                {
                    MovePlayerHorizontaly(_direction);
                }
            }
        }

        private bool AreInputArrowsUp()
        {
            return Input.GetKeyUp(Settings.MoveLeft) || Input.GetKeyUp(Settings.MoveRight);
        }

        private void MovePlayerHorizontaly(int direction)
        {
            var playerPos = transform.position;
            playerPos.x += direction * Settings.MoveSpeed * Time.deltaTime;
            transform.position = playerPos;
        }

        private void UpdateStunTime()
        {
            _timeSinceStunned += Time.deltaTime;
            if (_timeSinceStunned >= Settings.StunTime)
            {
                _timeSinceStunned = 0;
                _isStunned = false;
                IssueEvent(OnEndCurrent);
            }
        }

        private void DecrementHeightIndex()
        {
            _previousHeightIndex = _currentHeightIndex;
            _currentHeightIndex = Mathf.Max(_currentHeightIndex - 1, 0);
        }

        private void IncrementHeightIndex()
        {
            _previousHeightIndex = _currentHeightIndex;
            _currentHeightIndex = Mathf.Min(_currentHeightIndex + 1, Settings.Heights.Positions.Length - 1);
        }

        private void MovePlayerVertically()
        {
            var desiredHeight = Settings.Heights.Positions[_currentHeightIndex].y;
            transform.DOLocalMoveY(desiredHeight, Settings.VerticalMoveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(ProcessHeightReached);
            _isMovingVertically = true;
        }

        private void ProcessHeightReached()
        {
            _isMovingVertically = false;
            CheckForEndConditions();
            CheckIfShouldStun();
        }

        private void CheckIfShouldStun()
        {
            if(_previousHeightIndex - _currentHeightIndex > 0)
            {
                Stun();
            }
        }

        private void CheckForEndConditions()
        {
            if (_currentHeightIndex == 0)
            {
                IssueEvent(OnGroundReached);
            }
            else if (_currentHeightIndex == Settings.Heights.Positions.Length - 1)
            {
                _godMode = true;
                IssueEvent(OnTopReached);
            }
        }

        public bool CanMoveUp()
        {
            if(HasHole(Vector2.up) || _godMode)
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

        private bool HasHole(Vector2 direction)
        {
            return Physics2D.Raycast(transform.position, direction, Settings.RayDistance, Settings.HoleLayerMask);
        }

        private void IssueEvent(PlayerEventHanlder eventToIssue)
        {
            if(eventToIssue != null)
            {
                eventToIssue(GetComponent<Player>());
            }
        }
    }
}