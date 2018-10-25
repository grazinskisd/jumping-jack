using UnityEngine;

namespace JumpingJack
{
    public class HazardsController: AutoMotionController
    {
        public int HazardMovingDirection;
        public Vector2 HazardPositionOffset;

        protected override void Start()
        {
            _direction = HazardMovingDirection;
            base.Start();
        }

        protected override Vector2 GetRandomPosition()
        {
            return base.GetRandomPosition() + HazardPositionOffset;
        }

        protected override Vector2 GetNextSpawnPosition(AutoMotion sender)
        {
            return base.GetNextSpawnPosition(sender) + HazardPositionOffset;
        }
    }
}
