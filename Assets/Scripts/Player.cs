using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public class Player : MonoBehaviour
    {
        public PlayerSettings PlayerSettings;

        private int _currentHeightIndex;

        private void Start()
        {
            _currentHeightIndex = 0;
        }

        private void Update()
        {
            var playerPos = transform.position;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _currentHeightIndex = Mathf.Min(_currentHeightIndex + 1, PlayerSettings.Heights.Positions.Length - 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _currentHeightIndex = Mathf.Max(_currentHeightIndex - 1, 0);
            }

            playerPos.y = PlayerSettings.Heights.Positions[_currentHeightIndex].y;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerPos.x -= PlayerSettings.HorizontalDispozition;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerPos.x += PlayerSettings.HorizontalDispozition;
            }

            transform.position = playerPos;
        }
    }
}