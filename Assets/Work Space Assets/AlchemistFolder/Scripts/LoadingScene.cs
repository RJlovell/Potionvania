using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public string sceneNameToLoad;
    [SerializeField] private Image progressBar;
    // Start is called before the first frame update
    void Start()
    {
        //Start async operation
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        //create an async operation]
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(sceneNameToLoad);
        while(gameLevel.progress < 1)
        {
            //take the progress bar fill = async operation progress.

        }
        //when finished, load the game scene;
        yield return new WaitForEndOfFrame();
    }
}
