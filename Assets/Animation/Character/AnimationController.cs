﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationController : MonoBehaviour
{
    Animator anim;
    PlayerHealth health;
    Player playerScript;
    // Start is called before the first frame update
    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        anim.SetBool("isDead", false);
        anim.SetBool("midAir", false);
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.A) && playerScript.grounded) || (Input.GetKey(KeyCode.D) && playerScript.grounded))
        {
            anim.SetBool("isWalking", true); anim.SetFloat("movement", 1f);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        { 
            anim.SetBool("isWalking", false); anim.SetFloat("movement", 0f); 
        }

        if (health.playerCurrentHealth == 0)
        {
            anim.SetBool("isDead", true);
        }
 
        if (Input.GetKeyUp(KeyCode.Space))
            anim.SetTrigger("jumpStart");
        if ((playerScript.grounded == false) && (playerScript.potionLaunch == false))
            anim.SetBool("midAir", true);
        if ((playerScript.grounded == true) && (playerScript.potionLaunch == false))
            anim.SetTrigger("jumpEnd");
        if (playerScript.potionLaunch == true)
            anim.SetTrigger("potionLaunched");
        if (playerScript.potionLaunch == false)
            anim.SetTrigger("regainControl");
        if (Input.GetKeyUp(KeyCode.Mouse0))
            anim.SetTrigger("throwPotion");
       
    }
}