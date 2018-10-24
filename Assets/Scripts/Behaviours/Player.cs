﻿using UnityEngine;

namespace JumpingJack
{
    public delegate void PlayerEventHanlder(Player sender);

    public class Player : MonoBehaviour
    {
        public PlayerSettings Settings;

        public event PlayerEventHanlder OnJump;
        public event PlayerEventHanlder OnGroundReached;
        public event PlayerEventHanlder OnTopReached;

        private int _previousHeightIndex;
        private int _currentHeightIndex;
        private bool _isMovingVertically;

        private void Start()
        {
            _currentHeightIndex = 0;
            _previousHeightIndex = 0;
            _isMovingVertically = false;
        }

        private void Update()
        {
            var playerPos = transform.position;

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
                }
            }
            else
            {
                // Process input
                if (Input.GetKeyDown(KeyCode.UpArrow) && CanMoveUp())
                {
                    _previousHeightIndex = _currentHeightIndex;
                    _currentHeightIndex = Mathf.Min(_currentHeightIndex + 1, Settings.Heights.Positions.Length - 1);
                    _isMovingVertically = true;
                    IssueEvent(OnJump);
                }
                else if (ShouldFallDown())
                {
                    _previousHeightIndex = _currentHeightIndex;
                    _currentHeightIndex = Mathf.Max(_currentHeightIndex - 1, 0);
                    _isMovingVertically = true;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    playerPos.x -= Settings.MoveSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.RightArrow))
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
            if(Physics2D.Raycast(transform.position, Vector2.up, Settings.RayDistance))
            {
                return true;
            }
            return false;
        }

        private bool ShouldFallDown()
        {
            if (Physics2D.Raycast(transform.position, Vector2.down, Settings.RayDistance))
            {
                return true;
            }
            return false;
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