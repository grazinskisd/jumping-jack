﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpingJack
{
    public class IntroController: MonoBehaviour
    {
        public float WaitTime;

        public void Start()
        {
            StartCoroutine(LoadGameScene());
            PlayerPrefsService.ResetToStart();
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}
