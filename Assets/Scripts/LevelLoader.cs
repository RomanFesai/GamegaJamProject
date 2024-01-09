using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LevelLoader : MonoBehaviour
    {
        public Animator transition;

        private static LevelLoader instance;

        public float transitionTime = 1f;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("Found more than one Level Loader in the scene");
            }
            instance = this;
        }

        public static LevelLoader GetInstance()
        {
            return instance;
        }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        public void LoadLevelByName(string loadLevelName)
        {
            StartCoroutine(loadLevelByName(loadLevelName));
        }

        public void RestartCurrentLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        }

        IEnumerator LoadLevel(int levelIndex)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(levelIndex);
        }

        IEnumerator loadLevelByName(string loadLevelName)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(loadLevelName);
        }
    }
}