using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;

    public bool iSceneEnabled = false;
    public float iSceneDuration;
    public float iSceneCountdown;
    //ISceneScript invinciScript;

    private void Start()
    {
        iSceneCountdown = iSceneDuration;
    }
    // Update is called once per frame
    void Update()
    {
        if (iSceneEnabled)
        {
            iSceneCountdown -= Time.deltaTime;
        }
        if (iSceneCountdown <= 0f && iSceneEnabled)
        {
            iSceneEnabled = false;
            iSceneCountdown = iSceneDuration;
            print("iSceneCountdown is now reset & iScene is disabled");
        }
    }
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
    }
}
