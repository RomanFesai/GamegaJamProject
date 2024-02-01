using Assets.Scripts.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PauseMenuScripts
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused = false;
        public GameObject PauseWindow;
        [SerializeField] private PlayerMovementCC playerMovement;
        [SerializeField] private MouseLook playerCamera;
        //[SerializeField] private Animator menuAnim;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !GameOverWindow.isGameOver)
            {
                if (GameIsPaused && Time.timeScale == 0)
                {
                    Resume();
                }
                else if(!GameIsPaused && Time.timeScale == 1)
                {
                    Pause();
                }
            }
        }
        public void ReturnToMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Resume()
        {
            
            playerMovement.enabled = true;
            playerCamera.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            StartCoroutine(MenuOff());
        }

        public void Pause()
        {
            playerMovement.enabled = false;
            playerCamera.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            StartCoroutine(MenuOn());
        }

        IEnumerator MenuOn()
        {
            PauseWindow.SetActive(true);
            GameIsPaused = true;
            //menuAnim.SetBool("PauseMenuOn", true);
            yield return new WaitForSeconds(0f);
            Time.timeScale = 0;
        }

        IEnumerator MenuOff()
        {
            Time.timeScale = 1;
            //menuAnim.SetBool("PauseMenuOn", false);
            yield return new WaitForSeconds(0f);
            PauseWindow.SetActive(false);
            GameIsPaused = false;
        }
    }
}