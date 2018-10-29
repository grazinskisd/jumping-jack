using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;

namespace JumpingJack
{
    public delegate void PlayerEventHanlder(Player sender);

    public class Player : MonoBehaviour
    {
        public PlayerSettings Settings;
        public bool GodMode;
        [Header("Camera shake properties")]
        public float BadJumpDelay = 0.5f;
        public float ShakeDuration = 0.2f;
        public int ShakeStrength = 1;
        public int ShakeVibrato = 100;

        public event PlayerEventHanlder OnJump;
        public event PlayerEventHanlder OnStun;
        public event PlayerEventHanlder OnRunLeft;
        public event PlayerEventHanlder OnRunRight;
        public event PlayerEventHanlder OnBadJump;
        public event PlayerEventHanlder OnEndCurrent;

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
        private Vector3 _cameraPosition;
        private bool _isPlayerWrapping;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
            _camera = Camera.main;
            _cameraPosition = _camera.transform.position;
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

        private void Stun(float delay = 0)
        {
            _isStunned = true;
            _timeSinceStunned = 0;
            IssueEvent(OnStun);

            if(delay > 0)
            {
                StartCoroutine(DelayedCameraShake(delay));
            }
            else
            {
                CameraShake();
            }
        }

        private IEnumerator DelayedCameraShake(float delay)
        {
            yield return new WaitForSeconds(delay);
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
        }

        private void CameraShake()
        {
            _camera.DOShakePosition(ShakeDuration, ShakeStrength, ShakeVibrato)
                .OnComplete(() => _camera.transform.position = _cameraPosition);
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
                    Stun(BadJumpDelay);
                    IssueEvent(OnBadJump);
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
            if (!_isStunned)
            {
                if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                {
                    Debug.Log("FADADADAD");
                    IssueEvent(OnEndCurrent);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    MovePlayerHorizontaly(-1);
                    IssueEvent(OnRunLeft);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    MovePlayerHorizontaly(1);
                    IssueEvent(OnRunRight);
                }
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