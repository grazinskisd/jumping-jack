using UnityEngine;

namespace JumpingJack
{
    public class Player : MonoBehaviour
    {
        public PlayerSettings Settings;

        private int _currentHeightIndex;
        private bool _isMovingVertically;

        private void Start()
        {
            _currentHeightIndex = 0;
            _isMovingVertically = false;
        }

        private void Update()
        {
            var playerPos = transform.position;

            if (_isMovingVertically)
            {
                var desiredHeight = Settings.Heights.Positions[_currentHeightIndex].y;
                int direction = (int)((desiredHeight - playerPos.y) / Mathf.Abs(desiredHeight - playerPos.y));
                playerPos.y += direction * Settings.VerticalSpeed * Time.deltaTime;
                if(direction * (playerPos.y - desiredHeight) > 0)
                {
                    playerPos.y = desiredHeight;
                    _isMovingVertically = false;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && CanMoveUp())
                {
                    _currentHeightIndex = Mathf.Min(_currentHeightIndex + 1, Settings.Heights.Positions.Length - 1);
                    _isMovingVertically = true;
                }
                else if (ShouldFallDown())
                {
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

            if(Mathf.Abs(playerPos.x) >= Settings.RightEndPosition)
            {
                playerPos.x = -1 * (playerPos.x / Mathf.Abs(playerPos.x)) * Settings.RightEndPosition;
            }
            transform.position = playerPos;
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
    }
}