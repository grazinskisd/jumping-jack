using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpingJack
{
    public class EndSceneController: MonoBehaviour
    {
        private const string SCORE_TEXT_FORMAT = "FINAL  SCORE  {0:00000}\nWITH  {1}  HAZARDS";

        public Text ScoreText;
        public GameObject NewHighPanel;

        private void Start()
        {
            UpdateScores();
            PlayerPrefsService.ResetToStart();
        }

        private void UpdateScores()
        {
            int score = PlayerPrefsService.GetInt(Prefs.Score);
            int hazards = PlayerPrefsService.GetInt(Prefs.Hazards);
            int highscore = PlayerPrefsService.GetInt(Prefs.Highscore);

            ScoreText.text = string.Format(SCORE_TEXT_FORMAT, score, hazards);
            NewHighPanel.SetActive(false);
            if (score > highscore)
            {
                PlayerPrefsService.SetInt(Prefs.Highscore, score);
                NewHighPanel.SetActive(true);
            }
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
