using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrowing : MonoBehaviour
{
    public GameObject player;
    Vector3 worldPosition;
    Rigidbody rb;
    float launchAngle;

    public float throwForce = 1.0f;
    bool applyForce = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 12;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 throwVec = new Vector3((worldPosition.x - rb.transform.position.x), (worldPosition.y - rb.transform.position.y));
        float mag = GetMag(throwVec.x, throwVec.y);
        throwVec.x /= mag;
        throwVec.y /= mag;
        throwVec *= throwForce;

        launchAngle = Mathf.Rad2Deg * Mathf.Atan((worldPosition.x - rb.transform.position.x) / (worldPosition.y - rb.transform.position.y));
        if(launchAngle > 90)
        {
            //constraints as to not throw ball at self. -90 to 90 degrees only
        }
        if (applyForce)
        {
            rb.AddForce(throwVec, ForceMode.Impulse);
            applyForce = false;
        }
    }
    
    public static float GetMag(float first, float second)
    {
        float mag = Mathf.Sqrt((first * first) + (second * second));
        return mag;
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
