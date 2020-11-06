using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AirPotion : MonoBehaviour
{
    public float radius = 2.0f;
    Vector3 explosionVec;
    public float explosionForce = 7.0f;
    float magnitude;
    private Player playerScript;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public static float GetMag(float first, float second)
    {
        float mag = Mathf.Sqrt(((first * first) + (second * second)));
        return mag;
    }

    void OnCollisionEnter(Collision other)
    {
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliderList = Physics.OverlapSphere(explosionPos, radius);//check for colliders in explosion radius
            foreach (Collider hit in colliderList)
            {
                if (hit.CompareTag("Potion"))
                {
                    continue;
                }
                if (hit.CompareTag("Player"))
                {
                    playerScript.potionLaunch = true;
                }

                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null) //if the collided object has a rigid body, generate distance vector between potion impact point and collided rigid body.
                {
                    explosionVec = new Vector3(rb.transform.position.x - explosionPos.x, (rb.transform.position.y + 1) - explosionPos.y, 0);

                    //normalise vector
                    magnitude = GetMag(explosionVec.x, explosionVec.y);
                    explosionVec.x /= magnitude;
                    explosionVec.y /= magnitude;
                    //apply explosion force
                    explosionVec *= explosionForce;
                    explosionVec += rb.velocity;

                    //add force to rigid body
                    Vector3 newVelocity = rb.velocity;
                    newVelocity.y = 0;
                    rb.velocity = newVelocity;
                    rb.AddForce(explosionVec, ForceMode.Impulse);
                }
            }
        }
    }
}