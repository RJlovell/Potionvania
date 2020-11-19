using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    Animator anim;
    PlayerHealth health;
    // Start is called before the first frame update
    void Start()
    {
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        anim.SetBool("isDead", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            anim.SetBool("isWalking", true);
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            anim.SetBool("isWalking", false);
        if(health.playerCurrentHealth == 0)
        {
            anim.SetBool("isDead", true);
        }
    }
}
