using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    PlayerHealth playerHPScript;
    OrcScript orcScript;
    public float speed;
    //The maximum distance of the ray from the raycast offset
    //Cannot be 1 or less than 1 otherwise it will just be colliding with itself, 
    //causing the object to move back and forth for infinity.
    public float maxRayDistance;

    public float xAxisForce;
    public float yAxisForce;
    //Determines how far the ray cast should detect collisions from the object
    public float horizontalRayCastOffset;
    public float downwardsRayCastOffset;

    protected bool dealDamage = false;
    private bool movingRight = true;

    RaycastHit hitInfo;
    Ray horizontalDetectRay;
    Ray groundDetectRay;
    private void Start()
    {
        //playerHPScript = GameObject.Find("Player").GetComponent<PlayerHealth>();
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        orcScript = GetComponent<OrcScript>();
    }

    void FixedUpdate()
    {

        /*Creates a ray cast using the position of the game object this script is attached to plus
        a ternary operator using the variable movingRight to determine the direction of the raycast while the
        raycastOffset determines how far the ray will be from the game objects position. 
        The ray will be in a downwards direction.
        */
        groundDetectRay = new Ray(transform.position + new Vector3(movingRight ? downwardsRayCastOffset : -downwardsRayCastOffset, 0, 0), Vector3.down);

        if (movingRight)
        {
            //Similar to the groundDetectRay but the ray will be in a right direction
            horizontalDetectRay = new Ray(transform.position + new Vector3(movingRight ? horizontalRayCastOffset : -horizontalRayCastOffset, 0, 0), Vector3.right);
            dealDamage = false;

            Debug.DrawRay(horizontalDetectRay.origin, Vector3.right * maxRayDistance, Color.red);
            Debug.DrawRay(groundDetectRay.origin, Vector3.down, Color.blue);

            //The object is moving right
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            //The ray will be in a left direction
            horizontalDetectRay = new Ray(transform.position + new Vector3(movingRight ? horizontalRayCastOffset : -horizontalRayCastOffset, 0, 0), Vector3.left);
            Debug.DrawRay(horizontalDetectRay.origin, Vector3.left * maxRayDistance, Color.red);
            Debug.DrawRay(groundDetectRay.origin, Vector3.down, Color.blue);
            dealDamage = false;
            //The object is moving left
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        //If the raycast doesn't return that ground is being hit, then it will do the insides
        //But if the raycast does return that the ground is being hit, it will do nothing.
        if (!Physics.Raycast(groundDetectRay, out hitInfo, maxRayDistance))
        {
            movingRight = !movingRight;
        }
        ///A horizontal ray cast that detects if another object is blocking the game object this script is attached to.
        ///If the game object is being blocked it will begin moving in the opposite direction.
        ///If the game object collides with another object which has the "Player" tag, 
        ///this game object will ignore all physics and pass through the "Player" object, 
        ///dealing damage before continuing on the path.
        if (Physics.Raycast(horizontalDetectRay, out hitInfo, maxRayDistance))
        {
            if (hitInfo.collider.gameObject.CompareTag("Player"))
            {
                if (!playerHPScript.iSceneEnabled)
                {
                    //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), hitInfo.collider, false);
                    //hitInfo.collider.attachedRigidbody.AddForce(movingRight ? xAxisForce : -xAxisForce, yAxisForce, 0);
                    playerHPScript.iSceneEnabled = true;

                    if (!dealDamage)
                    {
                        print("Collided with " + hitInfo.collider.gameObject.name);
                        print("Orc dealt " + orcScript.orcDamage);
                        playerHPScript.TakeDamage(orcScript.orcDamage);
                        dealDamage = true;
                    }
                }
            }
            else
            {
                movingRight = !movingRight;
                dealDamage = false;
            }
        }
        //if (hitInfo.collider.gameObject.CompareTag("Player"))
        //{
        //    if (playerHPScript.iSceneEnabled)
        //    {
        //        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), hitInfo.collider, true);
        //    }
        //    else
        //    {
        //        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), hitInfo.collider, false);
        //    }
        //}
        //if (playerHPScript.iSceneEnabled)
        //{
        //    Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), hitInfo.collider, true);
        //}
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.CompareTag("Player"))
        {
            if (playerHPScript.iSceneEnabled)
            {
                Debug.Log("Collision has occured from the Orc Patrol script");
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), col.collider, true);
                col.collider.attachedRigidbody.AddForce(movingRight ? xAxisForce : -xAxisForce, yAxisForce, 0);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(playerHPScript.iSceneEnabled)
            {
                Debug.Log("Trigger has occured from the Orc Patrol script");
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.gameObject.GetComponent<Collider>(), true);
                other.gameObject.GetComponent<Collider>().attachedRigidbody.AddForce(movingRight ? xAxisForce : -xAxisForce, yAxisForce, 0);
                //col.collider.attachedRigidbody.AddForce(movingRight ? xAxisForce : -xAxisForce, yAxisForce, 0);
            }
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), col.collider, false);
        }
    }
}