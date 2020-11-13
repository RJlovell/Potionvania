using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlatformMovement : MonoBehaviour
{
    public Vector3[] points;
    //public float pointNumber;
    private Vector3 currentTarget;

    //These two variables are not required to be altered
    [SerializeField] private int pointNumber;
    [SerializeField] private float tolerance;
    public float speed;

    public float delayTime;
    private float delayStart;

    public bool automaticMovement;
    //public bool manualMovement;
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
        //if(platformMoving)
        //{
        //
        //}
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Orc") || other.CompareTag("Goblin"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Orc") || other.CompareTag("Goblin"))
        {
            other.transform.SetParent(null);
        }
    }
}
