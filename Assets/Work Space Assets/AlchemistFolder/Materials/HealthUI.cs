using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    PlayerHealth playerHealth;
    protected int health;
    public int numberOfHearts;
    
    //public int testHealth;
    //public int testNumberOfHearts;

    public RawImage[] hearts;
    public Texture heart;
    public Texture noHeart;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        //health = playerHealth.playerCurrentHealth;
        numberOfHearts = playerHealth.playerMaxHealth;
    }

    private void Update()
    {
        health = playerHealth.playerCurrentHealth;
        if(health > numberOfHearts)
        {
            health = numberOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].texture = heart;
            }
            else
            {
                hearts[i].texture = noHeart;
            }
            if(i < numberOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
