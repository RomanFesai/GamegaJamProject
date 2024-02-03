//using Assets.Scripts.PauseMenuScripts;
using Assets.Scripts.SaveLoad;
//using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Translation;

namespace Assets.Scripts.MainMenu
{
    public class MainMenu : MonoBehaviour/*, IDataPersistence*/
    {
        //[SerializeField] private GameObject continueButton;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject settingsSubMenu;
        [SerializeField] private GameObject creditsSubMenu;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown languageDropdown;

        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;
        private List<string> languages = new List<string> {"English", "Русский"};
        string scene;
        private void Start()
        {
            //PauseMenu.GameIsPaused = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            InitResolutions();

           /* if (!DataPersistenceManager.instance.HasGameData())
            {
                continueButton.SetActive(false);
            }*/
        }
        public void OnNewGameClicked()
        {
            //DataPersistenceManager.instance.NewGame();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnLoadGameClicked()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //DataPersistenceManager.instance.isLoaded = true;
            SceneManager.LoadSceneAsync(scene);
            //LevelLoader.GetInstance().LoadLevelByName(scene);
            //SceneManager.LoadSceneAsync("LightHouse");
        }

        public void OnSettingsButtonClicked()
        {
            mainMenu.SetActive(false);
            settingsSubMenu.SetActive(true);
        }
        public void OnCreditsButtonClicked()
        {
            mainMenu.SetActive(false);
            creditsSubMenu.SetActive(true);
        }
        public void CreditsReturnButtonClicked()
        {
            mainMenu.SetActive(true);
            creditsSubMenu.SetActive(false);
        }
        public void SettingsReturnButtonClicked()
        {
            mainMenu.SetActive(true);
            settingsSubMenu.SetActive(false);
        }

        public void setFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void InitResolutions()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                if(resolutions[i].refreshRateRatio.value == Screen.currentResolution.refreshRateRatio.value)
                    filteredResolutions.Add(resolutions[i]);
            }

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height;
                options.Add(option);

                if (filteredResolutions[i].width == Screen.currentResolution.width && filteredResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }

            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void InitLanguages()
        {
            languageDropdown.AddOptions(languages);
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetLanguage(int languageIndex)
        {
            Language.instance.currentLanguage = languageDropdown.options[languageIndex].text;
            Debug.Log(Language.instance.currentLanguage);
        }

        /*public void LoadData(GameData data)
        {
            scene = data.scene;
        }

        public void SaveData(ref GameData data)
        {
        }*/
    }
}