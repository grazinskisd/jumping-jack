using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpingJack
{
    public class GlobalInput: MonoBehaviour
    {
        private static bool _isCreated = false;

        public void Awake()
        {
            if (!_isCreated)
            {
                DontDestroyOnLoad(gameObject);
                _isCreated = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(Scenes.IntroScene.ToString());
            }
        }
    }
}
