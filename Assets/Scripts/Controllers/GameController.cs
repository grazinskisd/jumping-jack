using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

namespace JumpingJack
{
    public delegate void GameEventHandler();

    public class GameController: MonoBehaviour
    {
        public LifeController LifeController;
        public PlayerController Player;
        public float SceneLoadDelay;

        public event GameEventHandler OnWin;
        public event GameEventHandler OnLose;

        private PlayerPrefsService _prefService;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            LifeController.OnZeroLives += ProcessLose;
            Player.OnTopReached += ProcessWin;
        }

        private void ProcessWin(PlayerController sender)
        {
            _prefService.Increment(Prefs.Level, 1);
            if (!_prefService.WasLastLevel())
            {
                _prefService.Increment(Prefs.Hazards, 1);
            }
            IssueEvent(OnWin);
            LoadSceneDelayed(Scenes.NextLevelScene, SceneLoadDelay);
        }

        private void ProcessLose()
        {
            IssueEvent(OnLose);
            LoadSceneDelayed(Scenes.EndScene, SceneLoadDelay);
        }

        private void LoadSceneDelayed(Scenes scene, float delay)
        {
            StartCoroutine(LoadDelayed(scene, delay));
        }

        private IEnumerator LoadDelayed(Scenes scene, float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(scene.ToString());
        }

        private void IssueEvent(GameEventHandler gameEvent)
        {
            if(gameEvent != null)
            {
                gameEvent();
            }
        }
    }
}
