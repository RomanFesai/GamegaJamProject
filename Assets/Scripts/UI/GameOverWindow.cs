using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class GameOverWindow : MonoBehaviour
    {
        public static bool isGameOver = false;

        public bool IsGameOver { get => isGameOver; set => isGameOver = value; }

        private void Start()
        {
            isGameOver = true;
            //Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        public void Retry()
        {
            //Time.timeScale = 1f;
            isGameOver = false;
            Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}