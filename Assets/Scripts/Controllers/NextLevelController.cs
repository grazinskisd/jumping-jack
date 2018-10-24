using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpingJack
{
    public class NextLevelController: MonoBehaviour
    {
        private const string NEXT_LEVEL_FORMAT = "NEXT LEVEL - {0} HAZARD";

        public Text NextLevelText;
        public Text PhymeText;

        public float WaitTime;

        public void Start()
        {
            UpdateHazardsText();
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
