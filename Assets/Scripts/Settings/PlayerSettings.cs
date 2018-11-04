using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/PlayerSettings")]
    public class PlayerSettings: ScriptableObject
    {
        [Header("Moving properties")]
        public PositionsScriptable Heights;
        public float MoveSpeed;
        public KeyCode MoveLeft;
        public KeyCode MoveRight;
        public KeyCode MoveUp;
        [Tooltip("How far up to check for hole.")]
        public float RayDistance;
        public float RightEndPosition;
        public float RightSpawnPosition;
        public LayerMask HoleLayerMask;
        public float StunTime;
    }
}
