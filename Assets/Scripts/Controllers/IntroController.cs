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

        private int _level = 0;

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
        /// Pressing letter A will increase the level to be loaded.
        /// </summary>
        private void CheckForCheatInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _level = Mathf.Min(_level + 1, 20);
                Debug.Log("Level: " + _level);
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
