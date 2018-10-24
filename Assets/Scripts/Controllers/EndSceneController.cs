using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpingJack
{
    public class EndSceneController: MonoBehaviour
    {
        private void Start()
        {
            PlayerPrefsService.ResetToStart();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene(Scenes.GameScene.ToString());
            }
        }
    }
}
