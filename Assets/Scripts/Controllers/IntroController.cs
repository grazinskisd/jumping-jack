using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpingJack
{
    public class IntroController: MonoBehaviour
    {
        private const string LVL_CHEAT_FORMAT = "Lvl: {0}";

        public float WaitTime;
        public AppearingUILetters AppearingText;
        public AudioSource AudioSource;
        public AudioClip LetterRaise;
        public PeopleLetters PeopleLetters;
        public Text CheatText;

        private int _level = 0;
        private PlayerPrefsService _prefService;
        private bool _isLevelCheatActivated;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            CheatText.gameObject.SetActive(false);
            AppearingText.OnFinished += LoadGame;
            _prefService.ResetToStart();
            PeopleLetters.OnLettersRaising += OnLettersRaise;
            PeopleLetters.OnAllRaised += AppearingText.StartDisplaying;
        }

        private void Update()
        {
            CheckForCheatInput();
        }

        public void OnLettersRaise()
        {
            AudioSource.PlayOneShot(LetterRaise);
        }

        /// <summary>
        /// Pressing letter 'A' will increase the level to be loaded.
        /// Pressing letter 'G' will enable god mode
        /// </summary>
        private void CheckForCheatInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _level = Mathf.Min(_level + 1, _prefService.MaxLevel);
                if (!_isLevelCheatActivated)
                {
                    ActivateLvlCheatDisplay();
                }
                CheatText.text = string.Format(LVL_CHEAT_FORMAT, _level);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                _prefService.SetInt(Prefs.GodMode, 1);
            }

            if (Input.GetKeyDown(KeyCode.Return) && _isLevelCheatActivated)
            {
                LoadGameImmediate();
            }
        }

        private void ActivateLvlCheatDisplay()
        {
            CheatText.gameObject.SetActive(true);
            _isLevelCheatActivated = true;
        }

        private void LoadGame()
        {
            if (!_isLevelCheatActivated)
            {
                StartCoroutine(LoadGameScene());
            }
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            LoadGameImmediate();
        }

        private void LoadGameImmediate()
        {
            _prefService.SetLevelTo(_level);
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}
