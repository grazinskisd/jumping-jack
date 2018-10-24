using UnityEngine;

namespace JumpingJack
{
    public class ScoreController: MonoBehaviour
    {
        private const string HIGHSCORE_FORMAT = "HI{0:00000}";
        private const string SCORE_FORMAT = "SC{0:00000}";
        private const string HIGHSCORE_PRAM = "HI";

        public Player Player;
        public TextMesh HighscoreText;
        public TextMesh ScoreText;
        public int ScoreIncrement;

        private int _currentScore;
        private int _currentHighscore;

        private void Start()
        {
            _currentHighscore = PlayerPrefs.GetInt(HIGHSCORE_PRAM);
            HighscoreText.text = string.Format(HIGHSCORE_FORMAT, _currentHighscore);
            UpdateScoreText();
            Player.OnJump += IncrementScore;
        }

        private void IncrementScore(Player sender)
        {
            _currentScore += ScoreIncrement;
            UpdateScoreText();
            CheckIfNewHighscore();
        }

        private void CheckIfNewHighscore()
        {
            if(_currentScore > _currentHighscore)
            {
                PlayerPrefs.SetInt(HIGHSCORE_PRAM, _currentScore);
                _currentHighscore = _currentScore;
            }
        }

        private void UpdateScoreText()
        {
            ScoreText.text = string.Format(HIGHSCORE_FORMAT, _currentScore);
        }
    }
}
