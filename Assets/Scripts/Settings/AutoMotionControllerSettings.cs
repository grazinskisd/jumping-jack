using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/AutoMotionControllerSettings")]
    public class AutoMotionControllerSettings : ScriptableObject
    {
        public int NumberOfObjectsOnStart;
        public int ObjectCountCap;
        public PositionsScriptable Heights;
        public MovingObjectSettings ObjectSettings;
    }
}
