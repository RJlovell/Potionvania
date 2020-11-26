using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int  playerMaxHealth;
    [System.NonSerialized]public int playerCurrentHealth;
    
    [System.NonSerialized]public bool iSceneEnabled = false;
    public float iSceneDuration;
    private float iSceneCountdown;
    private float deathAnimCount = 5f;
    private bool deathAnim = false;
    private int death = 0;

    public bool damageTaken = false;

    GameCondition gameCon;

    Animator anim;//ref for animator

    Player player;

    private void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        iSceneCountdown = iSceneDuration;
        player = gameObject.GetComponent<Player>();

        gameCon = gameObject.GetComponent<GameCondition>();
        anim = GetComponent<Animator>(); //get compnent for animator
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
            damageTaken = false;
            iSceneEnabled = false;
            iSceneCountdown = iSceneDuration;
            print("iSceneCountdown is now reset & iScene is disabled");
        }
        IsPlayerDead();
        if (deathAnim)
        {
            if (death == 0)
            {
                anim.SetTrigger("isDead");
                death = 1;

            }
            deathAnimCount -= Time.deltaTime;
            if (deathAnimCount <= 0f)
            {
                

                playerCurrentHealth = playerMaxHealth;
                SceneManager.LoadScene(gameCon.defeatSceneName);
                anim.SetBool("isDead", false);
                deathAnim = false;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        playerCurrentHealth -= damage;
        damageTaken = true;
        if (playerCurrentHealth >0 )
            anim.SetTrigger("takeDamage");
    }

    public bool IsPlayerDead()
    {
        if(playerCurrentHealth <= 0)
        {
            //GameObject .setActive == false
            //This means if player goes below 0 they will die
            //Game Manager will change the scene to either the defeat screen or just restart the level
            //gameObject.SetActive(false);
            player.dead = true;
            Debug.Log("The player has died");
            deathAnim = true;
            
            return true;
          
        }
        return false;
    }
}
