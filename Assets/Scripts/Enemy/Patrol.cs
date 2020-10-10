using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float maxRayDistance;
    public float raycastOffset = 0.5f;

    private bool movingRight = true;
    bool hitWall = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Debug.Log("Object has triggered");
            hitWall = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Debug.Log("Wall collision occured");
            hitWall = true;
        }
    }

    void Update()
    {
        /*Creates a ray cast using the position of the game object this script is attached to plus
        a ternary operator using the variable movingRight to determine the direction of the raycast while the
        raycastOffset determine how far the ray will be from the game objects position. 
        The ray will be in a downwards direction.
        */
        Ray theRay = new Ray(transform.position + new Vector3(movingRight ? raycastOffset : -raycastOffset, 0, 0), Vector3.down);
        if (movingRight)
        {
            //The object is moving right
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            //The object is moving left
            transform.Translate(-transform.right * speed * Time.deltaTime);
        }

        RaycastHit groundInfo;
        //If the raycast doesn't return that ground is being hit, then it will do the insides
        //But if the raycast does return that the ground is being hit, it will do nothing.
        if (!Physics.Raycast(theRay, out groundInfo, maxRayDistance) || hitWall)
        {
            //Debug.DrawLine(theRay.origin, theRay.direction, Color.blue);
            movingRight = !movingRight;
            hitWall = false;

        }
    }
}