using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcPatrolSensor : MonoBehaviour
{
    PlayerHealth playerHPScript;
    //OrcPatrol orcPatrol;
    OrcPatrol orcPatrolParent;
    
    Collider orcColliderInfo;
    Collider playerColliderInfo;
    //bool movementRight;
    public bool moveThrough = false;

    
    private void Start()
    {
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        //orcPatrol = GameObject.FindGameObjectWithTag("Orc").GetComponent<OrcPatrol>();
        orcPatrolParent = GetComponentInParent<OrcPatrol>();

        orcColliderInfo = GameObject.FindGameObjectWithTag("Orc").gameObject.GetComponent<Collider>();
        playerColliderInfo = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerHPScript.iSceneEnabled)
            {
                playerHPScript.iSceneEnabled = true;
                moveThrough = true;
                Debug.Log("Trigger has occured from the Orc Patrol Sensor script");
            }
        }
        //else
        //{
        //    orcPatrolParent.movingRight = !orcPatrolParent.movingRight;
        //    //orcPatrol.movingRight = !orcPatrol.movingRight;
        //    moveThrough = false;
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            orcPatrolParent.movingRight = !orcPatrolParent.movingRight;
            //orcPatrol.movingRight = !orcPatrol.movingRight;
            moveThrough = false;
        }
    }

    private void Update()
    {
        
        if (moveThrough)
        {
            Debug.Log("Move through is true");
            Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, true);

            //playerColliderInfo.attachedRigidbody.AddForce(orcPatrolParent.movingRight ? orcPatrolParent.xAxisForce : -orcPatrolParent.xAxisForce, orcPatrolParent.yAxisForce, 0);


        }

        if(!moveThrough)
        {
            Debug.Log("The other player object is not colliding with the orc collider anymore");

            Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, false);
        }
    }

}
