using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCondition : MonoBehaviour
{
    public string victorySceneName;
    public string defeatSceneName;
    public string retryGameName;
    public string mainMenuName;

    public void Victory()
    {
        SceneManager.LoadScene(victorySceneName);
    }

    public void Defeat()
    {
        SceneManager.LoadScene(defeatSceneName);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(retryGameName);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuName);
    }
}
