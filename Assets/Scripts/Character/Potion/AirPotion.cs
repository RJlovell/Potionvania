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
    public float potionLaunchEffectHeight = 1; //how high from feet level does the potion launch push
    public bool appliedToPlayer = false;

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        playerScript.potionExists = true;
    }
    void OnDestroy()
    {
        playerScript.potionExists = false;
    }

    void OnCollisionEnter(Collision other)
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliderList = Physics.OverlapSphere(explosionPos, radius);//check for colliders in explosion radius
        foreach (Collider hit in colliderList)
        {
            if (hit.CompareTag("Player") && appliedToPlayer)
                continue;
            if (hit.CompareTag("Potion") || hit.CompareTag("Orc"))
                continue;
            if (hit.CompareTag("Player"))
            {
                playerScript.potionLaunch = true;
                appliedToPlayer = true;
            }


            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null) //if the collided object has a rigid body, generate distance vector between potion impact point and collided rigid body.
            {
                rb.velocity = Vector3.zero;
                explosionVec = new Vector3(rb.transform.position.x - explosionPos.x, (rb.transform.position.y + potionLaunchEffectHeight) - explosionPos.y, 0);

                //normalise vector
                magnitude = GetMag(explosionVec.x, explosionVec.y);
                explosionVec.x /= magnitude;
                explosionVec.y /= magnitude;
                //apply explosion force
                explosionVec *= explosionForce;
                //zero velocity then add force to rigid body
/*                float vel = Mathf.Sqrt(explosionVec.x * explosionVec.x + explosionVec.y * explosionVec.y);
                while (vel > explosionForce)
                {
                    explosionVec.x -= 0.1f;
                    explosionVec.y -= 0.1f;
                    vel = Mathf.Sqrt(explosionVec.x * explosionVec.x + explosionVec.y * explosionVec.y);
                }*/
                rb.AddForce(explosionVec, ForceMode.VelocityChange);
            }
        }
    }
    public static float GetMag(float first, float second)
    {
        float mag = Mathf.Sqrt(((first * first) + (second * second)));
        return mag;
    }
}