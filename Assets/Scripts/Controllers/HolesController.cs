using System.Collections.Generic;
using UnityEngine;

namespace JumpingJack
{
    public class HolesController: AutoMotionController
    {
        public Player Player;

        private List<AutoMotion> _holes = new List<AutoMotion>();
        private int _holeCount = 0;

        protected override void Start()
        {
            _direction = 1;
            base.Start();
            Player.OnJump += SpawnRandomHole;
        }

        protected override void UpdateDirection()
        {
            _direction *= -1;
        }

        private void SpawnRandomHole(Player sender)
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
