using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/MovingObjectSettings")]
    public class MovingObjectSettings: ScriptableObject
    {
        public float MoveSpeed;
        public float RightEnd;
        [Tooltip("Distance before end to spawn the next hole")]
        public float SpawnDistance;
    }
}
