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
        IsPlayerDead();
    }
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
    }

    public bool IsPlayerDead()
    {
        if(playerHealth <= 0)
        {
            //GameObject .setActive == false
            //This means if player goes below 0 they will die
            //Game Manager will change the scene to either the defeat screen or just restart the level
            playerHealth = 0;
            Debug.Log("The player has died");
            return true;
        }
        return false;
    }
}
