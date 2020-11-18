using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    Player player;
    public static bool isPaused = false;
    public string mainMenuSceneName, settingsSceneName;
    public static bool gameIsPaused = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if(isPaused)
        {
            gameIsPaused = false;
            Time.timeScale = 1;
            //For when the player or the camera listens out for the audio in the scene
            //Plays the music in the scene when the pause menu is deactivated
            //AudioListener.pause = false;
            pauseMenuUI.SetActive(false);
            isPaused = false;
            ///This line is coded to ensure that when the player selects the resume button, 
            ///it will not spawn a potion at the position of the mouses click.
            if(player.canThrow)
            {
                player.canThrow = false;
                player.timeSinceThrow = player.throwDelay - 0.1f;
            }
        }
        else
        {
            gameIsPaused = true;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            //Pauses the music in the scene in the pause menu is enabled.
            //AudioListener.pause = true;
            isPaused = true;
        }
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
