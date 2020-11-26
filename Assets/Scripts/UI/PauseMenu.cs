using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject howToPlayUI;
    Player player;
    public string mainMenuSceneName, settingsSceneName;
    public static bool gameIsPaused = false;

    private float timer = 0.0f;
    public float timeLimit = 0.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if (!player.canThrow && !gameIsPaused && !player.potionExists)
        {
            if (timer < timeLimit)
            {
                timer += Time.deltaTime;

            }
            if (timer >= timeLimit)
            {
                timer = 0;
                player.canThrow = true;
            }
        }
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        if (!gameIsPaused)
        {
            Time.timeScale = 1;
            //For when the player or the camera listens out for the audio in the scene
            //Plays the music in the scene when the pause menu is deactivated
            //AudioListener.pause = false;
            pauseMenuUI.SetActive(false);
            optionsMenuUI.SetActive(false);
            howToPlayUI.SetActive(false);
        }
        else
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0;
            ///This line is coded to ensure that when the player selects the resume button, 
            ///it will not spawn a potion at the position of the mouses click.
            if (player.canThrow)
            {
                player.canThrow = false;
                timer = 0;
            }
            //Pauses the music in the scene in the pause menu is enabled.
            //AudioListener.pause = true;
        }
    }

    public void LoadMainMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
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
