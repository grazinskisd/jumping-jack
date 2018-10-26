using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/PlayerSettings")]
    public class PlayerSettings: ScriptableObject
    {
        public PositionsScriptable Heights;
        public float VerticalMoveDuration;
        public float MoveSpeed;
        [Tooltip("How far up to check for hole.")]
        public float RayDistance;
        public float RightEndPosition;
        public float RightSpawnPosition;
        public LayerMask HoleLayerMask;
        public float StunTime;
    }
}
