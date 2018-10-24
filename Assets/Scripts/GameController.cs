using UnityEngine.SceneManagement;
using UnityEngine;

namespace JumpingJack
{
    public class GameController: MonoBehaviour
    {
        public LifeController LifeController;

        private void Start()
        {
            LifeController.OnZeroLives += RestartLevel;
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
