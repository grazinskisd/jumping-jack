using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpingJack
{
    public class NextLevelController: MonoBehaviour
    {
        private const string NEXT_LEVEL_FORMAT = "NEXT LEVEL - {0} HAZARD";

        public BaladSettings Balad;
        public AppearingUILetters AppearingText;
        public Text NextLevelText;
        public Text PhymeText;

        public float WaitTime;

        public void Start()
        {
            UpdateHazardsText();
            AppearingText.FullText = Balad.Rhyme[PlayerPrefsService.GetInt(Prefs.Level) - 1];
            AppearingText.StartDisplaying();
            AppearingText.OnFinished += LoadGame;
        }

        private void LoadGame()
        {
            StartCoroutine(LoadGameScene());
        }

        private void UpdateHazardsText()
        {
            string format = NEXT_LEVEL_FORMAT;
            int hazards = PlayerPrefsService.GetInt(Prefs.Hazards);
            if (hazards != 1)
            {
                format += "S";
            }
            NextLevelText.text = string.Format(format, hazards);
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}
