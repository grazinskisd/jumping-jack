using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/PlayerSettings")]
    public class PlayerSettings: ScriptableObject
    {
        public PositionsScriptable Heights;
        public float HorizontalDispozition;
    }
}
