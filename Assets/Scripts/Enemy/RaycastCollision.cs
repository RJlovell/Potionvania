using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCollision : MonoBehaviour
{
    public float movingForce;
    Vector3 startPoint;
    Vector3 origin;
    public int NoOfRays = 10;
    int i;
    RaycastHit hitInfo;
    float lengthOfRay, distanceBetweenRays, directionFactor;
    float margin = 0.015f;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        //Length of the Ray is distance from the center to edge
        lengthOfRay = GetComponent<Collider>().bounds.extents.x;

        //Initialise directionFactor for x-axis direction
        directionFactor = Mathf.Sign(Vector3.right.x);
    }

    // Update is called once per frame
    void Update()
    {
        //First ray origin point for this frame
        startPoint = new Vector3(GetComponent<Collider>().bounds.min.x + margin, transform.position.y, transform.position.z);
        //startPoint = new Vector3(transform.position.x, GetComponent<Collider>().bounds.min.y + margin,  transform.position.z);
        if (!IsCollidingHorizontally())
        {
            transform.Translate(Vector3.right * movingForce * Time.deltaTime * directionFactor);
        }
    }
    bool IsCollidingHorizontally()
    {
        origin = startPoint;
        distanceBetweenRays = (GetComponent<Collider>().bounds.size.x - 2 * margin / (NoOfRays - 1));
        for (i = 0; i < NoOfRays; i++)
        {
            //Ray to be casted.
            ray = new Ray(origin, Vector3.right * directionFactor);
            //Draw ray on screen to see visually. Remember visual length is not actual length.
            Debug.DrawRay(origin, Vector3.right * directionFactor, Color.red);
            if (Physics.Raycast(ray, out hitInfo, lengthOfRay))
            {

                print("Collided with " + hitInfo.collider.gameObject.name);
                //Negate the directionFactor to reverse the moving direction of colliding cube(here cube2)
                directionFactor = -directionFactor;
                return true;
            }
            origin += new Vector3(distanceBetweenRays, 0, 0);

        }
        return false;
    }
}
