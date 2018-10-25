using System;
using UnityEngine;

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

        private void Start()
        {
            _currentHeightIndex = 0;
            _previousHeightIndex = 0;
            _isMovingVertically = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!GodMode)
            {
                _isStunned = true;
            }
        }

        private void Update()
        {
            var playerPos = transform.position;

            if (_isStunned)
            {
                _timeSinceStunned += Time.deltaTime;
                if (_timeSinceStunned >= Settings.StunTime)
                {
                    _timeSinceStunned = 0;
                    _isStunned = false;
                }
            }

            if (_isMovingVertically)
            {
                // Gradually move player up or down
                var desiredHeight = Settings.Heights.Positions[_currentHeightIndex].y;
                int direction = (_currentHeightIndex - _previousHeightIndex) / Mathf.Abs(_currentHeightIndex - _previousHeightIndex);
                playerPos.y += direction * Settings.VerticalSpeed * Time.deltaTime;
                if(direction * (playerPos.y - desiredHeight) > 0)
                {
                    playerPos.y = desiredHeight;
                    _isMovingVertically = false;
                    CheckForEndConditions();
                    CheckIfShouldStun();
                }
            }
            else
            {
                // Process input
                if (Input.GetKeyDown(KeyCode.UpArrow) && !_isStunned)
                {
                    if (CanMoveUp())
                    {
                        _previousHeightIndex = _currentHeightIndex;
                        _currentHeightIndex = Mathf.Min(_currentHeightIndex + 1, Settings.Heights.Positions.Length - 1);
                        _isMovingVertically = true;
                        IssueEvent(OnJump);
                    }
                    else
                    {
                        _isStunned = true;
                    }
                }
                else if (ShouldFallDown())
                {
                    _previousHeightIndex = _currentHeightIndex;
                    _currentHeightIndex = Mathf.Max(_currentHeightIndex - 1, 0);
                    _isMovingVertically = true;
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && !_isStunned)
                {
                    playerPos.x -= Settings.MoveSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.RightArrow) && !_isStunned)
                {
                    playerPos.x += Settings.MoveSpeed * Time.deltaTime;
                }
            }

            // Wrap player movement
            if(Mathf.Abs(playerPos.x) >= Settings.RightEndPosition)
            {
                playerPos.x = -1 * (playerPos.x / Mathf.Abs(playerPos.x)) * Settings.RightEndPosition;
            }
            transform.position = playerPos;
        }

        private void CheckIfShouldStun()
        {
            if(_previousHeightIndex - _currentHeightIndex > 0)
            {
                _isStunned = true;
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
            if(HasHole(Vector2.up) || GodMode)
            {
                return true;
            }
            return false;
        }

        private bool ShouldFallDown()
        {
            if (HasHole(Vector2.down) && !GodMode)
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