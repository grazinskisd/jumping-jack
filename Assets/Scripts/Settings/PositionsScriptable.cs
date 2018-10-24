using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/Positions")]
    public class PositionsScriptable : ScriptableObject
    {
        public Vector2[] Positions;
    }
}