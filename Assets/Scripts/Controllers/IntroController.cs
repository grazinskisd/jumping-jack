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
        private PlayerPrefsService _prefService;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            AppearingText.StartDisplaying();
            AppearingText.OnFinished += LoadGame;
            _prefService.ResetToStart();
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
#if UNITY_EDITOR
                Debug.Log("Level: " + _level);
#endif
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _prefService.SetInt(Prefs.GodMode, 1);
            }
        }

        private void LoadGame()
        {
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            _prefService.SetLevelTo(_level);
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}
