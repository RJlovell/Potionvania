using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int  playerMaxHealth;
    public int playerCurrentHealth;
    
    public bool iSceneEnabled = false;
    public float iSceneDuration;
    public float iSceneCountdown;

    GameCondition gameCon;

    private void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        iSceneCountdown = iSceneDuration;

        gameCon = gameObject.GetComponent<GameCondition>();
    }
    // Update is called once per frame
    void Update()
    {
        if(playerCurrentHealth > playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
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
    public void TakeDamage(int damage)
    {
        playerCurrentHealth -= damage;
    }

    public bool IsPlayerDead()
    {
        if(playerCurrentHealth <= 0)
        {
            //GameObject .setActive == false
            //This means if player goes below 0 they will die
            //Game Manager will change the scene to either the defeat screen or just restart the level
            //gameObject.SetActive(false);
            Debug.Log("The player has died");
            playerCurrentHealth = playerMaxHealth;
            SceneManager.LoadScene(gameCon.defeatSceneName);
            return true;
        }
        return false;
    }
}
