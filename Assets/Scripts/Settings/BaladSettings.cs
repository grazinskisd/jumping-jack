using UnityEngine;

namespace JumpingJack
{
    [CreateAssetMenu(menuName = "JumpingJack/Balad")]
    public class BaladSettings: ScriptableObject
    {
        [TextArea]
        public string[] Rhyme;
    }
}
