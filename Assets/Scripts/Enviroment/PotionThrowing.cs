using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrowing : MonoBehaviour
{
    private Player playerScript;
    public GameObject player;
    Rigidbody rb;

    public float throwForce = 1.0f;
    bool applyForce = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GameObject.Find("Bottlehead").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 throwVec = playerScript.potionVel * throwForce;

       
        if (applyForce)
        {
            //Debug.Log(launchAngle);
            rb.AddForce(throwVec, ForceMode.Impulse);
            applyForce = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
