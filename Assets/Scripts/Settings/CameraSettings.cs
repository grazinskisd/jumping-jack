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
    }
}
