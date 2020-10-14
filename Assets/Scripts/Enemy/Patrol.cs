using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    OrcScript orcScript;
    public float speed;
    //The maximum distance of the ray from the raycast offset
    //Cannot be 1 or less than 1 otherwise it will just be colliding with itself, 
    //causing the object to move back and forth for infinity.
    public float maxRayDistance;

    //Determines how far the ray cast should detect collisions from the object
    public float horizontalRayCastOffset;
    public float downwardsRayCastOffset;
    
    private bool movingRight = true;

    RaycastHit hitInfo;
    Ray horizontalDetectRay;
    Ray groundDetectRay;
    private void Start()
    {
        orcScript = GetComponent<OrcScript>();
    }

    void Update()
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
            Debug.DrawRay(horizontalDetectRay.origin, Vector3.right * maxRayDistance, Color.red);
            Debug.DrawRay(groundDetectRay.origin, Vector3.down, Color.blue);
            
            //The object is moving right
            //transform.Translate(transform.right * speed * Time.deltaTime);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            //The ray will be in a left direction
            horizontalDetectRay = new Ray(transform.position + new Vector3(movingRight ? horizontalRayCastOffset : -horizontalRayCastOffset, 0, 0), Vector3.left);
            Debug.DrawRay(horizontalDetectRay.origin, Vector3.left * maxRayDistance, Color.red);
            Debug.DrawRay(groundDetectRay.origin, Vector3.down, Color.blue);
            //Debug.DrawLine(transform.position, Vector3.right * maxRayDistance, Color.gray);
            
            //The object is moving left
            //transform.Translate(-transform.right * speed * Time.deltaTime);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        //If the raycast doesn't return that ground is being hit, then it will do the insides
        //But if the raycast does return that the ground is being hit, it will do nothing.
        if (!Physics.Raycast(groundDetectRay, out hitInfo, maxRayDistance))
        {
            
            //Debug.DrawLine(groundDetectRay.origin, groundDetectRay.direction, Color.blue);
            movingRight = !movingRight;
        }
        
        if (Physics.Raycast(horizontalDetectRay, out hitInfo, maxRayDistance))
        {
            print("Collided with " + hitInfo.collider.gameObject.name);
            if(hitInfo.collider.gameObject.tag == "Player")
            {
                hitInfo.collider.attachedRigidbody.AddForce(-10000,0,0);
                //orcScript.orcDamage;
                print("Orc dealt " + orcScript.orcDamage);
            }
            movingRight = !movingRight;
        }
        
    }
}