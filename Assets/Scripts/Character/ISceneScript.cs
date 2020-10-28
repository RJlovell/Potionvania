using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISceneScript : MonoBehaviour
{
    public bool iSceneEnabled = false;

    public float iSceneDuration;

    public float iSceneCountdown;
    // Start is called before the first frame update
    void Start()
    {
        iSceneCountdown = iSceneDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if(iSceneEnabled)
        {
            iSceneCountdown -= Time.deltaTime;
        }
        if(iSceneCountdown <= 0f && iSceneEnabled)
        {
            iSceneEnabled = false;
            iSceneCountdown = iSceneDuration;
            print("IScene is now disabled & iScene is disabled");
        }
    }
}
