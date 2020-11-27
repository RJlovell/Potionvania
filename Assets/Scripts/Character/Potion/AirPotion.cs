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
    bool impacted = false;
    public GameObject[] potionImpacts = new GameObject[6];
    int num = 0;

    Ray blockCheck;
    RaycastHit hitInfo;
    bool blocked = false;
    bool directHit = false; //checks if goblin is hit directly due to direct collision proving difficult for the engine to calculate normally

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        playerScript.potionExists = true;
    }


    void OnCollisionEnter(Collision other)
    {
        if (!impacted) //impacted is a bool which controls this loop only activating once per potion, fixing the case where the potion hits a corner and registers 2 OnCollisionEnters before destroying
        {
            ///Randomly generate one of thr 6 options for explosion animation/sound
            num = UnityEngine.Random.Range(1, 6);
            //instantiate Onomatopoeia and potion explosion animation
            Instantiate(potionImpacts[num - 1], transform.position, Quaternion.Euler(0, 90, -5));
            
            impacted = true;
        }

        if (other.gameObject.CompareTag("Goblin")) //checking colliding with a goblin directly
        {
            directHit = true;
            //check which side the potion hit the goblin and add a set force and diagonal direction
            if (transform.position.x < other.transform.position.x)
            {
                explosionVec = Vector3.up + Vector3.right;
                explosionVec *= explosionForce;
                other.gameObject.GetComponent<Rigidbody>().AddForce(explosionVec, ForceMode.VelocityChange);
            }
            else
            {
                explosionVec = Vector3.up + Vector3.left;
                explosionVec *= explosionForce;
                other.gameObject.GetComponent<Rigidbody>().AddForce(explosionVec, ForceMode.VelocityChange);
            }
        }

        Vector3 explosionPos = transform.position; //save position of impact
        Collider[] colliderList = Physics.OverlapSphere(explosionPos, radius);//check for colliders in explosion radius
        foreach (Collider hit in colliderList)
        {
            blocked = false;
            if (hit.CompareTag("Player") && appliedToPlayer)//if Player has already had force added as per bool condition, continue to next collider in list
                continue;
            if (hit.CompareTag("Potion") || hit.CompareTag("Orc"))//do not add force to other potions or Orcs
                continue;
            if (hit.CompareTag("Player")) //if player is in radius, potion launch is true and appliedToPlayer is true to stop seconf force
            {
                playerScript.potionLaunch = true;
                appliedToPlayer = true;
            }
            if(hit.CompareTag("Goblin")) //if goblin is in explosion radius, check if there is anything between explosionPosition and GoblinPosition to block force
            {
                if (directHit)
                    continue;
                Vector3 dir = new Vector3(hit.transform.position.x - explosionPos.x, (hit.transform.position.y + potionLaunchEffectHeight) - explosionPos.y, 0);
                magnitude = GetMag(dir.x, dir.y);
                dir.x /= magnitude;
                dir.y /= magnitude;
                blockCheck = new Ray(explosionPos, dir);
                Debug.DrawRay(blockCheck.origin, blockCheck.direction, Color.blue, 2);
                if(Physics.Raycast(blockCheck, out hitInfo))
                {
                    if (!hitInfo.rigidbody)
                        continue;
                }
            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && !blocked) //if the collided object has a rigid body, generate distance vector between potion impact point and collided rigid body.
            {
                rb.velocity = Vector3.zero;
                explosionVec = new Vector3(rb.transform.position.x - explosionPos.x, (rb.transform.position.y + potionLaunchEffectHeight) - explosionPos.y, 0);
                

                //normalise vector
                magnitude = GetMag(explosionVec.x, explosionVec.y);
                explosionVec.x /= magnitude;
                explosionVec.y /= magnitude;
                //apply explosion force
                explosionVec *= explosionForce;
                //add force to rigid body
                rb.AddForce(explosionVec, ForceMode.VelocityChange);
            }
        }
    }
    public static float GetMag(float first, float second) //custom GetMag function for no particular reason (So I'm told)
    {
        float mag = Mathf.Sqrt(((first * first) + (second * second)));
        return mag;
    }

    void OnDestroy()
    {
        playerScript.potionExists = false;    
    }

}