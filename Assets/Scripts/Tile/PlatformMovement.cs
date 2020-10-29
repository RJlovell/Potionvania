using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformMovement : MonoBehaviour
{
    public Vector3[] points;
    public int pointNumber;
    //public float pointNumber;
    private Vector3 currentTarget;

    public float tolerance;
    public float speed;
    public float delayTime;

    private float delayStart;

    public bool automaticMovement;
    private bool platformMoving;
    // Start is called before the first frame update
    void Start()
    {
        if(points.Length > 0)
        {
            currentTarget = points[0];
        }
        tolerance = speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition != currentTarget)
        {

            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    void MovePlatform()
    {
        Vector3 movingDirection = currentTarget - transform.localPosition;
        transform.localPosition += (movingDirection / movingDirection.magnitude) * speed * Time.deltaTime;
        if(movingDirection.magnitude < tolerance)
        {
            transform.localPosition = currentTarget;
            delayStart = Time.time;
        }
    }

    void UpdateTarget()
    {
        if(automaticMovement)
        {
            if(Time.time - delayStart > delayTime)
            {
                NextPlatform();
            }
        }
    }
    public void NextPlatform()
    {
        pointNumber++;
        if(pointNumber >= points.Length)
        {
            pointNumber = 0;
        }
        currentTarget = points[pointNumber];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Orc") || collision.gameObject.CompareTag("Goblin"))
        {
            //platformMoving = true;
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Orc") || collision.gameObject.CompareTag("Goblin"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
