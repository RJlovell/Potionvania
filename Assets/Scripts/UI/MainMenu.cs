using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string playSceneName;
    public string settingsSceneName;
    public void PlayGame()
    {
        SceneManager.LoadScene(playSceneName);
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene(settingsSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
