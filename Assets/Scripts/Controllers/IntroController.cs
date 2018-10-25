using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpingJack
{
    public class IntroController: MonoBehaviour
    {
        public float WaitTime;
        public AppearingUILetters AppearingText;

        private int _level = 1;

        private void Start()
        {
            AppearingText.StartDisplaying();
            AppearingText.OnFinished += LoadGame;
            PlayerPrefsService.ResetToStart();
        }

        private void Update()
        {
            CheckForCheatInput();
        }

        /// <summary>
        /// Pressing letter 'A' will increase the level to be loaded.
        /// Pressing letter 'G' will enable god mode
        /// </summary>
        private void CheckForCheatInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _level = Mathf.Min(_level + 1, 20);
                Debug.Log("Level: " + _level);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                PlayerPrefsService.SetInt(Prefs.GodMode, 1);
            }
        }

        private void LoadGame()
        {
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            PlayerPrefsService.SetLevelTo(_level);
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}
