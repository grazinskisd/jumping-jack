using UnityEngine;

namespace JumpingJack
{
    public class ScoreController: MonoBehaviour
    {
        private const string HIGHSCORE_FORMAT = "HI{0:00000}";
        private const string SCORE_FORMAT = "SC{0:00000}";

        public Player Player;
        public TextMesh HighscoreText;
        public TextMesh ScoreText;
        public int ScoreIncrement;

        private int _currentScore;
        private int _currentHighscore;
        private PlayerPrefsService _prefService;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            _currentHighscore = _prefService.GetInt(Prefs.Highscore);
            _currentScore = _prefService.GetInt(Prefs.Score);
            HighscoreText.text = string.Format(HIGHSCORE_FORMAT, _currentHighscore);
            UpdateScoreText();
            Player.OnJump += IncrementScore;
        }

        private void IncrementScore(Player sender)
        {
            _currentScore += ScoreIncrement * (_prefService.GetInt(Prefs.Level) + 1);
            _prefService.SetInt(Prefs.Score, _currentScore);
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            ScoreText.text = string.Format(SCORE_FORMAT, _currentScore);
        }
    }
}
