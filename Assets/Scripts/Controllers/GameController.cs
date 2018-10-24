using UnityEngine.SceneManagement;
using UnityEngine;

namespace JumpingJack
{
    public class GameController: MonoBehaviour
    {
        public LifeController LifeController;
        public Player Player;

        private void Start()
        {
            LifeController.OnZeroLives += RestartLevel;
            Player.OnTopReached += OnWin;
        }

        private void OnWin(Player sender)
        {
            PlayerPrefsService.IncrementPref(Prefs.Level, 1);
            PlayerPrefsService.IncrementPref(Prefs.Hazards, 1);
            SceneManager.LoadScene(Scenes.NextLevelScene.ToString());
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(Scenes.EndScene.ToString());
        }
    }
}
