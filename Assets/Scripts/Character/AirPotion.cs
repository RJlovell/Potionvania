﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AirPotion : MonoBehaviour
{
    public float radius = 2.0f;
    Vector3 explosionVec;
    public float explosionForce = 7.0f;
    float upwardsModifier = 0.0f;
    float magnitude;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) //mouse click simulating potion impact
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);//check for colliders in explosion radius
            foreach (Collider hit in colliders)
            {
                
                if (hit.CompareTag("Potion"))
                {
                    continue;
                }
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null) //if the collided object has a rigid body, generate distance vector between potion impact point and collided rigid body.
                {
                    explosionVec = new Vector3(rb.transform.position.x - explosionPos.x, rb.transform.position.y - explosionPos.y, 0);
                    //normalise vector
                    magnitude = GetMag(explosionVec.x, explosionVec.y);
                    explosionVec.x = explosionVec.x / magnitude;
                    explosionVec.y = explosionVec.y / magnitude;
                    //apply explosion force
                    explosionVec *= explosionForce;
                    //add force to rigid body
                    rb.AddForce(explosionVec, ForceMode.Impulse);
                }
            }
        }
    }
    public static float GetMag(float first, float second)
    {
        float mag = Mathf.Sqrt(((first * first) + (second * second)));
        return mag;
    }
}