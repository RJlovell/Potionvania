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
    public bool moveThrough = true;
    public bool orcPushBack = false;

    public float xAxisForce;
    public float yAxisForce;

    public float rayCastOffset;
    public float maxRayDistance;
    RaycastHit hitInfo;
    public Ray groundDetectRay;

    Vector3 explosionVec;
    public float explosionForce = 7.0f;
    public float minExplosionForce = 4.0f;
    float magnitude;
    Vector3 explosionPos;
    public float potionLaunchEffectHeight = 1;
    public float testOrcPushBackForce;


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
        if (other.CompareTag("Player"))
        {
            moveThrough = true;
            //player.potionLaunch = true;
            orcPushBack = true;
            //Vector3 newVelocity = other.attachedRigidbody.velocity;
            //newVelocity.x = 0;
            //newVelocity.y = 0;
            //other.attachedRigidbody.velocity = newVelocity;
            //other.attachedRigidbody.AddForce(orcPatrolParent.movingRight ? -xAxisForce : xAxisForce, yAxisForce, 0);

            /*explosionPos = transform.position;
            explosionVec = new Vector3((other.attachedRigidbody.transform.position.x + xAxisForce) - explosionPos.x, (other.attachedRigidbody.transform.position.y + yAxisForce) - explosionPos.y, 0);

            //normalise vector
            magnitude = AirPotion.GetMag(explosionVec.x, explosionVec.y);
            explosionVec.x /= magnitude;
            explosionVec.y /= magnitude;

            //explosionVec.Normalize();
            //apply explosion force
            explosionVec *= explosionForce;
            //zero velocity then add force to rigid body

            other.attachedRigidbody.AddForce(explosionVec, ForceMode.Impulse);*/
            Vector3 testPushBackForce = new Vector3(xAxisForce, yAxisForce, 0);
            //Vector3 dir = transform.position - other.transform.position;
            //dir.Normalize();
            Vector3 direction = other.ClosestPointOnBounds(other.transform.position) - transform.position;
            direction.Normalize();
            other.gameObject.GetComponent<Rigidbody>().AddForce(direction * testOrcPushBackForce);



            if (!playerHPScript.iSceneEnabled)
            {
                playerHPScript.iSceneEnabled = true;

                Debug.Log("Trigger has occured from the Orc Patrol Sensor script");
                if (!orcPatrolParent.dealDamage)
                {
                    print("Collided with " + other.gameObject.name);
                    print("Orc dealt " + orc.orcDamage);
                    //playerHPScript.TakeDamage(orc.orcDamage);
                    orcPatrolParent.dealDamage = true;
                }
            }
        }
        else if(other.CompareTag("Potion"))
        {
            moveThrough = true;
            player.potionLaunch = true;
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
            //moveThrough = false;
            Debug.Log("Trigger exit has occured from the Orc Patrol Sensor Script");
        }
    }

    private void Update()
    {
        groundDetectRay = new Ray(transform.position + new Vector3(orcPatrolParent.movingRight ? rayCastOffset : -rayCastOffset, 0, 0), Vector3.down);

        if (!Physics.Raycast(groundDetectRay, out hitInfo, maxRayDistance))
        {
            orcPatrolParent.movingRight = !orcPatrolParent.movingRight;
        }

        if (moveThrough)
        {
            Debug.Log("Move through is true");
            Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, true);
            //Physics.IgnoreCollision(orcTestingCollision, playerColliderInfo, true);
        }

        if(!moveThrough)
        {
            Debug.Log("The other player object is not colliding with the orc collider anymore");

            Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, false);
        }
    }
}
