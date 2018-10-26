using UnityEngine;
using UnityEngine.UI;

namespace JumpingJack
{
    public delegate void AppearingLettersEventHandler();

    public class AppearingUILetters : MonoBehaviour
    {
        [TextArea]
        public string FullText;
        public float LetterDelay;
        public Text TextField;
        public bool StartOnAwake;

        public event AppearingLettersEventHandler OnFinished;

        private float _timeSinceLastLetter;
        private string _currentText;
        private int _textLength;
        private int _currentIndex;

        private void Awake()
        {
            if (StartOnAwake)
            {
                StartDisplaying();
            }
        }

        public void StartDisplaying()
        {
            _textLength = FullText.Length;
            _currentIndex = 0;
            _currentText = "";
            _currentText += GetNextLetter();
            UpdateTextDisplay();
        }

        private string GetNextLetter()
        {
            char letter = FullText[_currentIndex];
            _currentIndex++;
            string result = "" + letter;
            while (char.IsWhiteSpace(letter) && !IsLastLetter())
            {
                letter = FullText[_currentIndex];
                result += letter;
                _currentIndex++;
            }
            return result;
        }

        private bool IsLastLetter()
        {
            return _currentIndex >= _textLength;
        }

        private void Update()
        {
            if (!IsLastLetter())
            {
                _timeSinceLastLetter += Time.deltaTime;
                if (_timeSinceLastLetter >= LetterDelay)
                {
                    _timeSinceLastLetter = 0;
                    _currentText += GetNextLetter();
                    UpdateTextDisplay();
                }

                if (IsLastLetter() && OnFinished != null)
                {
                    OnFinished();
                }
            }
        }

        private void UpdateTextDisplay()
        {
            TextField.text = _currentText;
        }
    }
}