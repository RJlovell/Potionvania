using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PotionThrowing : MonoBehaviour
{
    PotionEffect potionEffect;
    private Player playerScript;
    Rigidbody rb;
    float tempAngle = 0;
    float potionAngle;
    public float spinSpeed = 5;



    public float throwForce;
    bool applyForce = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        potionEffect = GameObject.FindGameObjectWithTag("Potion").GetComponent<PotionEffect>();
    }
    private void OnDestroy()
    {
        playerScript.throwCharge = playerScript.minThrowForce;
    }

    private void FixedUpdate()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), true);
        tempAngle += spinSpeed * throwForce;
        //potionAngle += 20;
        potionAngle = (rb.velocity.x >= 0 ? tempAngle * -1 : tempAngle);
        //transform.rotation = Quaternion.Euler(0, 0, potionAngle);
    }

    // Update is called once per frame
    void Update()
    {
        throwForce = playerScript.throwCharge;
        Vector3 throwVec = playerScript.potionVel * throwForce;
        if (applyForce)
        {
            //Debug.Log(launchAngle);
            rb.AddForce(throwVec, ForceMode.VelocityChange);
            applyForce = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        playerScript.canThrow = true;
        Destroy(gameObject);
    }
}
