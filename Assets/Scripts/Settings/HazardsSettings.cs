using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/HazardsSettings")]
    public class HazardsSettings: ScriptableObject
    {
        public AutoMotion[] Hazards;
        public Color[] Colors;
        public int MovingDirection;
        public Vector2 PositionOffset;
        [Tooltip("Delay for spawning hazard on the bottom level")]
        public float DelayForSpawn;
        public int PositionsPerPlatform;
    }
}
