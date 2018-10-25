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

        public void Start()
        {
            AppearingText.OnFinished += LoadGame;
            PlayerPrefsService.ResetToStart();
        }

        private void LoadGame()
        {
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}
