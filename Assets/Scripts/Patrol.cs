using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float maxRayDistance;
    public float raycastOffset = 0.5f;

    private bool movingRight = true;

    void Update()
    {
        //The ray :D!
        Ray theRay = new Ray(transform.position + new Vector3(movingRight ? raycastOffset : -raycastOffset, 0, 0), Vector3.down);
        if (movingRight)
        {
            //The object is moving right
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-transform.right * speed * Time.deltaTime);
        }

        RaycastHit groundInfo;

        if (!Physics.Raycast(theRay, out groundInfo, maxRayDistance))
        {
            Debug.DrawLine(theRay.origin, theRay.direction, Color.blue);
            movingRight = !movingRight;
        }
        else
        {
            Debug.DrawLine(theRay.origin, theRay.direction, Color.red);
        }
    }
}