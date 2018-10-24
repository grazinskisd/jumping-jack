using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JumpingJack
{
    public class NextLevelController: MonoBehaviour
    {
        public float WaitTime;

        public void Start()
        {
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitForSeconds(WaitTime);
            SceneManager.LoadScene(Scenes.GameScene.ToString());
        }
    }
}
