using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpingJack
{
    public class EndSceneController: MonoBehaviour
    {
        private const string SCORE_TEXT_FORMAT = "FINAL  SCORE  {0:00000}\nWITH  {1}  HAZARDS";

        public GameObject NewHighPanel;
        public AppearingUILetters NameText;
        public AppearingUILetters ScoreText;
        public AppearingUILetters NewHighText;
        public AppearingUILetters InstructionText;
        public float LetterDelay;

        private int _score;
        private int _hazards;
        private int _highscore;
        private PlayerPrefsService _prefService;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            SaveAndResetScores();
            SetDelays();
            NewHighPanel.SetActive(false);
            StartDisplayingName();
        }

        private void StartDisplayingName()
        {
            NameText.StartDisplaying();
            NameText.OnFinished += () => ProcessScore();
        }

        private void SaveAndResetScores()
        {
            _score = _prefService.GetInt(Prefs.Score);
            _hazards = _prefService.GetInt(Prefs.Hazards);
            _highscore = _prefService.GetInt(Prefs.Highscore);
            _prefService.ResetToStart();
        }

        private void SetDelays()
        {
            NameText.LetterDelay = LetterDelay;
            ScoreText.LetterDelay = LetterDelay;
            NewHighText.LetterDelay = LetterDelay;
            InstructionText.LetterDelay = LetterDelay;
        }

        private void ProcessScore()
        {
            DisplayScores();
            if (_score > _highscore)
            {
                ProcessNewHighscore(_score);
            }
            else
            {
                ShowInstructions();
            }
        }

        private void DisplayScores()
        {
            ScoreText.FullText = string.Format(SCORE_TEXT_FORMAT, _score, _hazards);
            ScoreText.StartDisplaying();
        }

        private void ShowInstructions()
        {
            ScoreText.OnFinished += () => InstructionText.StartDisplaying();
        }

        private void ProcessNewHighscore(int score)
        {
            _prefService.SetInt(Prefs.Highscore, score);
            DisplayNewHighPanel();
        }

        private void DisplayNewHighPanel()
        {
            NewHighPanel.SetActive(true);
            ScoreText.OnFinished += () => NewHighText.StartDisplaying();
            NewHighText.OnFinished += () => InstructionText.StartDisplaying();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene(Scenes.GameScene.ToString());
            }
        }
    }
}
