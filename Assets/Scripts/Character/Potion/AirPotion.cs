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
    public float minExplosionForce = 4.0f;
    float magnitude;
    private Player playerScript;
    public float potionLaunchEffectHeight = 1; //how high from feet level does the potion launch push
    public bool appliedToPlayer = false;

    Ray blockCheck;
    RaycastHit hitInfo;
    bool blocked = false;

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
            blocked = false;
            if (hit.CompareTag("Player") && appliedToPlayer)
                continue;
            if (hit.CompareTag("Potion") || hit.CompareTag("Orc"))
                continue;
            if (hit.CompareTag("Player"))
            {
                playerScript.potionLaunch = true;
                appliedToPlayer = true;
            }
            if(hit.CompareTag("Goblin"))
            {
                blockCheck = new Ray(explosionPos, hit.transform.position);
                if(Physics.Raycast(blockCheck, out hitInfo))
                {
                    if (hitInfo.collider.CompareTag("Untagged"))
                        continue;
                }
            }


            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && !blocked) //if the collided object has a rigid body, generate distance vector between potion impact point and collided rigid body.
            {
                rb.velocity = Vector3.zero;
                explosionVec = new Vector3(rb.transform.position.x - explosionPos.x, (rb.transform.position.y + potionLaunchEffectHeight) - explosionPos.y, 0);
                double distance = Math.Sqrt(Math.Pow(rb.transform.position.x - explosionPos.x, 2) + Math.Pow(rb.transform.position.y - explosionPos.y, 2));
                if (distance >= 1)
                {
                    distance = radius - distance;
                    distance /= radius;
                    explosionForce *= (float)distance;
                }
                if (explosionForce < minExplosionForce)
                    explosionForce = minExplosionForce;

                //normalise vector
                magnitude = GetMag(explosionVec.x, explosionVec.y);
                explosionVec.x /= magnitude;
                explosionVec.y /= magnitude;
                //apply explosion force
                explosionVec *= explosionForce;
                //zero velocity then add force to rigid body

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