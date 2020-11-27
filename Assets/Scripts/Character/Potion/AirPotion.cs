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
    public GameObject potionImpact1;
    public GameObject potionImpact2;
    public GameObject potionImpact3;
    public GameObject potionImpact4;
    public GameObject potionImpact5;
    public GameObject potionImpact6;
    public ParticleSystem potionTrail;

    Ray blockCheck;
    RaycastHit hitInfo;
    bool blocked = false;
    bool directHit = false; //checks if goblin is hit directly due to direct collision proving difficult for the engine to calculate normally

    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Instantiate(particleTrail);
       // particleTrail.transform.position = transform.localPosition;
       // Instantiate(potionTrail);
       
    }

    void Update()
    {
        playerScript.potionExists = true;
       // particleTrail.transform.position = transform.localPosition;
    }
    void OnDestroy()
    {
        playerScript.potionExists = false;
    }

    void OnCollisionEnter(Collision other)
    {

        int num = UnityEngine.Random.Range(1, 6);
        if (num == 1)
            Instantiate(potionImpact1, transform.position, Quaternion.Euler(0, 90, -5));
        if (num == 2)
            Instantiate(potionImpact2, transform.position, Quaternion.Euler(0, 90, -5));
        if (num == 3)
            Instantiate(potionImpact3, transform.position, Quaternion.Euler(0, 90, -5));
        if (num == 4)
            Instantiate(potionImpact4, transform.position, Quaternion.Euler(0, 90, -5));
        if (num == 5)
            Instantiate(potionImpact5, transform.position, Quaternion.Euler(0, 90, -5));
        if (num == 6)
            Instantiate(potionImpact6, transform.position, Quaternion.Euler(0, 90, -5));

        if (other.gameObject.CompareTag("Goblin"))
        {
            directHit = true;
            if(transform.position.x < other.transform.position.x)
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

            ///Randomly generate one of thr 6 options for explosion animation/sound
            
            


            if (rb != null && !blocked) //if the collided object has a rigid body, generate distance vector between potion impact point and collided rigid body.
            {
                rb.velocity = Vector3.zero;
                explosionVec = new Vector3(rb.transform.position.x - explosionPos.x, (rb.transform.position.y + potionLaunchEffectHeight) - explosionPos.y, 0);
                
               /* double distance = Math.Sqrt(Math.Pow(rb.transform.position.x - explosionPos.x, 2) + Math.Pow(rb.transform.position.y - explosionPos.y, 2));
                if (distance >= 1)
                {
                    distance = radius - distance;
                    distance /= radius;
                    explosionForce *= (float)distance;
                }
                if (explosionForce < minExplosionForce)
                    explosionForce = minExplosionForce;*/

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
    public static float GetMag(float first, float second)
    {
        float mag = Mathf.Sqrt(((first * first) + (second * second)));
        return mag;
    }

    
}