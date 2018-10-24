using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpingJack
{
    public class IntroController: MonoBehaviour
    {
        public string GameSceneName;
        public float WaitTime;

        public void Start()
        {
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
