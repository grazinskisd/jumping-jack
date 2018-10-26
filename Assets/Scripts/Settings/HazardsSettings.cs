using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/HazardsSettings")]
    public class HazardsSettings: ScriptableObject
    {
        public AutoMotion[] Hazards;
        public Color[] Colors;
    }
}
