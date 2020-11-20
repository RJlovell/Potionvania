using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcPatrol : MonoBehaviour
{
    protected PlayerHealth playerHPScript;
    OrcPatrolSensor orcSensor;
    public float speed;
    public float orcRotationAngle;

    //[System.NonSerialized]public bool movingRight;
    public bool movingRight;
    [System.NonSerialized]public bool dealDamage = false;

    private void Start()
    {
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        orcSensor = GetComponentInChildren<OrcPatrolSensor>();
    }

    void Update()
    {
        if (movingRight)
        {
            dealDamage = false;
            Debug.DrawRay(orcSensor.groundDetectRay.origin, Vector3.down * orcSensor.maxRayDistance, Color.green);
            //The object is moving right
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            dealDamage = false;
            Debug.DrawRay(orcSensor.groundDetectRay.origin, Vector3.down * orcSensor.maxRayDistance, Color.green);
            //The object is moving left
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(0, movingRight ? orcRotationAngle : -orcRotationAngle, 0);
    }
}