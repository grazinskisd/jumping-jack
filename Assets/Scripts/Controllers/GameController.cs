using UnityEngine.SceneManagement;
using UnityEngine;

namespace JumpingJack
{
    public class GameController: MonoBehaviour
    {
        public LifeController LifeController;
        public Player Player;

        private PlayerPrefsService _prefService;

        private void Awake()
        {
            _prefService = GameObject.FindObjectOfType<PlayerPrefsService>();
        }

        private void Start()
        {
            LifeController.OnZeroLives += LoadEndScene;
            Player.OnTopReached += OnWin;
        }

        private void OnWin(Player sender)
        {
            _prefService.Increment(Prefs.Level, 1);
            if (!_prefService.WasLastLevel())
            {
                _prefService.Increment(Prefs.Hazards, 1);
            }
            SceneManager.LoadScene(Scenes.NextLevelScene.ToString());
        }

        private void LoadEndScene()
        {
            SceneManager.LoadScene(Scenes.EndScene.ToString());
        }
    }
}
