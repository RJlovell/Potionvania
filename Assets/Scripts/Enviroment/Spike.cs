﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    PlayerHealth playerHPScript;

    public float xAxisPushBackForce;
    public float yAxisPushBackForce;
    // Start is called before the first frame update
    void Start()
    {
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!playerHPScript.iSceneEnabled)
            {
                Debug.Log("Player has taken damage from the spike");
                playerHPScript.playerHealth -= 1;
                playerHPScript.iSceneEnabled = true;
            }
            Vector3 newVelocity = other.attachedRigidbody.velocity;
            newVelocity.y = 0;
            other.attachedRigidbody.velocity = newVelocity;
            other.attachedRigidbody.AddForce(xAxisPushBackForce, yAxisPushBackForce, 0, ForceMode.Impulse);
            
        }
    }
}