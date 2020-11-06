using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isPaused = false;
    public string mainMenuSceneName, settingsSceneName;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                DeactivateMenu();
            }
            else
            {
                ActivateMenu();
            }
        }
    }

    public void ActivateMenu()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        //Pauses the music in the scene in the pause menu is enabled.
        //AudioListener.pause = true;
        isPaused = true;
    }
    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        //For when the player or the camera listens out for the audio in the scene
        //Plays the music in the scene when the pause menu is deactivated
        //AudioListener.pause = false;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene(settingsSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting application");
        Application.Quit();
    }
}
