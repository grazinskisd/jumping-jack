using UnityEngine;
using DG.Tweening;

namespace JumpingJack
{
    public delegate void PlayerEventHanlder(Player sender);

    public class Player : MonoBehaviour
    {
        public PlayerSettings Settings;
        public bool GodMode;

        public event PlayerEventHanlder OnJump;
        public event PlayerEventHanlder OnGroundReached;
        public event PlayerEventHanlder OnTopReached;

        private int _previousHeightIndex;
        private int _currentHeightIndex;
        private bool _isMovingVertically;

        private bool _isStunned;
        private float _timeSinceStunned;

        private bool _godMode;

        private PlayerPrefsService _prefService;
        private Camera _camera;
        private bool _isPlayerWrapping;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
            _camera = Camera.main;
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
            }
        }

        private void Stun()
        {
            _isStunned = true;
            _timeSinceStunned = 0;
            CameraShake();
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

            if (Input.GetKeyDown(KeyCode.S))
            {
                CameraShake();
            }
        }

        private void CameraShake()
        {
            _camera.DOShakePosition(0.2f, 1, 100).OnComplete(() => _camera.transform.position = Vector3.back * 10);
        }

        private void LateUpdate()
        {
            WrapPlayerMovement();
        }

        private void WrapPlayerMovement()
        {
            if (!_isPlayerWrapping && ShouldWrapPlayerPosition())
            {
                SlidePlayerOut();
            }
        }

        private bool ShouldWrapPlayerPosition()
        {
            return Mathf.Abs(transform.position.x) > Settings.RightEndPosition;
        }

        private void SlidePlayerOut()
        {
            Vector3 playerPos = transform.position;
            float sign = playerPos.x / Mathf.Abs(playerPos.x);
            float dist = Settings.RightSpawnPosition - Settings.RightEndPosition;
            float dur = dist / Settings.MoveSpeed;
            _isPlayerWrapping = true;

            transform.DOMoveX(sign * Settings.RightSpawnPosition, dur)
                .OnComplete(() => SlinePlayerIn(playerPos, sign, dur));
        }

        private void SlinePlayerIn(Vector3 playerPos, float sign, float dur)
        {
            playerPos.x = -sign * Settings.RightSpawnPosition;
            transform.position = playerPos;
            transform.DOMoveX(-sign * Settings.RightEndPosition, dur)
                .OnComplete(ResetIsPlayerWrapping);
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
            if (Input.GetKeyDown(KeyCode.UpArrow) && !_isStunned)
            {
                if (CanMoveUp())
                {
                    IncrementHeightIndex();
                    MovePlayerVertically();
                    IssueEvent(OnJump);
                }
                else
                {
                    Stun();
                }
            }
            else if (ShouldFallDown())
            {
                DecrementHeightIndex();
                MovePlayerVertically();
            }
        }

        private void ProcessHorizontalInput()
        {
            if (Input.GetKey(KeyCode.LeftArrow) && !_isStunned)
            {
                MovePlayerHorizontaly(-1);
            }
            else if (Input.GetKey(KeyCode.RightArrow) && !_isStunned)
            {
                MovePlayerHorizontaly(1);
            }
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
            transform.DOLocalMoveY(desiredHeight, Settings.VerticalMoveDuration).SetEase(Ease.Linear).OnComplete(ProcessHeightReached);
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
                IssueEvent(OnTopReached);
            }
        }

        private bool CanMoveUp()
        {
            if(HasHole(Vector2.up) || _godMode)
            {
                return true;
            }
            return false;
        }

        private bool ShouldFallDown()
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
                eventToIssue(this);
            }
        }
    }
}