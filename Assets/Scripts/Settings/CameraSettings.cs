using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/CameraSettings")]
    public class CameraSettings: ScriptableObject
    {
        public Color DefaultBackground;
        public Color BadJumpBackground;
        public Color HazardHitBackground;
        public float FlashDuration;
        [Header("Camera shake properties")]
        public float BadJumpDelay = 0.5f;
        public float ShakeDuration = 0.2f;
        public int ShakeStrength = 1;
        public int ShakeVibrato = 100;
    }
}
