using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/AudioSettings")]
    public class AudioSettings: ScriptableObject
    {
        [Range(0, 1)]
        public float Volume;
        [Header("Audio clips")]
        public AudioClip StandStraight;
        public AudioClip StandSide;
        public AudioClip Walk;
        public AudioClip Jump;
        public AudioClip Hazard;
        public AudioClip BadJump;
        public AudioClip Stun;
        public AudioClip Fall;
    }
}
