using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public class HolesController: AutoMotionController
    {
        public PlayerController Player;

        private List<AutoMotion> _holes = new List<AutoMotion>();
        private int _holeCount = 0;
        private int _startHeight = 0;

        protected override void Start()
        {
            _direction = 1;
            _startHeight = base.GetStartHeight();
            base.Start();
            Player.OnJump += SpawnRandomHole;
        }

        protected override int GetStartHeight()
        {
            return _startHeight;
        }

        protected override void UpdateDirection()
        {
            _direction *= -1;
        }

        private void SpawnRandomHole(PlayerController sender)
        {
            if (_holeCount < Settings.ObjectCountCap)
            {
                int holeIndex = Random.Range(0, Settings.Heights.Positions.Length);
                SetupNewObject(SpawnObject(_direction, holeIndex, GetRandomPosition()));
                UpdateDirection();
                _holeCount++;
            }
        }
    }
}
