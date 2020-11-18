using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcPatrolSensor : MonoBehaviour
{
    PlayerHealth playerHPScript;
    OrcPatrol orcPatrolParent;
    OrcScript orc;
    Player player;
    
    Collider orcColliderInfo;
    Collider playerColliderInfo;
    public bool moveThrough = false;

    public float xAxisForce;
    public float yAxisForce;

    private void Start()
    {
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        orcPatrolParent = GetComponentInParent<OrcPatrol>();
        orc = GetComponentInParent<OrcScript>();

        orcColliderInfo = GameObject.FindGameObjectWithTag("Orc").gameObject.GetComponent<Collider>();
        playerColliderInfo = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Collider>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Potion"))
        {
            moveThrough = true;
            player.potionLaunch = true;
            other.attachedRigidbody.AddForce(orcPatrolParent.movingRight ? xAxisForce : -xAxisForce, yAxisForce, 0);
            if (!playerHPScript.iSceneEnabled)
            {
                playerHPScript.iSceneEnabled = true;
    
                Debug.Log("Trigger has occured from the Orc Patrol Sensor script");
                if (!orcPatrolParent.dealDamage)
                        {
                            print("Collided with " + other.gameObject.name);
                            print("Orc dealt " + orc.orcDamage);
                            playerHPScript.TakeDamage(orc.orcDamage);
                            orcPatrolParent.dealDamage = true;
                        }
            }
        }
        else
        {
            orcPatrolParent.movingRight = !orcPatrolParent.movingRight;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moveThrough = false;
            Debug.Log("Trigger exit has occured from the Orc Patrol Sensor Script");
        }
    }

    private void Update()
    {
        if (moveThrough)
        {
            Debug.Log("Move through is true");
            Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, true);
        }

        if(!moveThrough)
        {
            Debug.Log("The other player object is not colliding with the orc collider anymore");

            Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, false);
        }
    }
}
