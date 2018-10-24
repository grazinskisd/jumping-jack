using UnityEngine;
using UnityEngine.UI;

namespace JumpingJack
{
    public class FlashingImageAndText: MonoBehaviour
    {
        public Image Image;
        public Text Text;

        public Color[] ImageColors;
        public Color[] TextColors;

        public float Delay = 1;

        private int _currentColor;
        private float _timeSinceLastChange;

        private void Start()
        {
            _currentColor = 0;
            _timeSinceLastChange = 0;
            UpdateImageColor();
        }

        private void UpdateImageColor()
        {
            Image.color = ImageColors[_currentColor];
            Text.color = TextColors[_currentColor];
        }

        private void Update()
        {
            _timeSinceLastChange += Time.deltaTime;
            if(_timeSinceLastChange >= Delay)
            {
                _currentColor = (_currentColor + 1) % Mathf.Min(ImageColors.Length, TextColors.Length);
                UpdateImageColor();
                _timeSinceLastChange = 0;
            }
        }
    }
}
