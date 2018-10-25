using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/PlayerSettings")]
    public class PlayerSettings: ScriptableObject
    {
        public PositionsScriptable Heights;
        public float MoveSpeed;
        public float VerticalSpeed;
        [Tooltip("How far up to check for hole.")]
        public float RayDistance;
        public float RightEndPosition;
        public LayerMask HoleLayerMask;
        public float StunTime;
    }
}
