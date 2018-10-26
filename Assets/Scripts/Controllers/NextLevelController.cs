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
        private const string S_LETTER = "S";

        public BaladSettings Balad;
        public AppearingUILetters AppearingText;
        public Text NextLevelText;
        public GameObject NextLevelGO;
        public Text PhymeText;

        public float WaitTime;

        private PlayerPrefsService _prefService;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        public void Start()
        {
            ProcessHazardsDisplay();
            AppearingText.FullText = Balad.Rhyme[_prefService.GetInt(Prefs.Level)-1];
            AppearingText.StartDisplaying();
            AppearingText.OnFinished += LoadGame;
        }

        private void ProcessHazardsDisplay()
        {
            if (_prefService.WasLastLevel())
            {
                DisableHazardsText();
            }
            else
            {
                UpdateHazardsText();
            }
        }

        private void DisableHazardsText()
        {
            NextLevelGO.SetActive(false);
        }

        private void LoadGame()
        {
            if (_prefService.WasLastLevel())
            {
                StartCoroutine(LoadGameScene(Scenes.EndScene.ToString()));
            }
            else
            {
                StartCoroutine(LoadGameScene(Scenes.GameScene.ToString()));
            }
        }

        private void UpdateHazardsText()
        {
            string format = NEXT_LEVEL_FORMAT;
            int hazards = _prefService.GetInt(Prefs.Hazards);
            if (hazards != 1)
            {
                format += S_LETTER;
            }
            NextLevelText.text = string.Format(format, hazards);
        }

        private IEnumerator LoadGameScene(string sceneName)
        {
            yield return new WaitForSeconds(WaitTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}
