using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PotionThrowing : MonoBehaviour
{
    PotionEffect potionEffect;
    private Player playerScript;
    Rigidbody rb;


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
        playerScript.throwCharge = playerScript.minThrowForce; //reset ThrowForce when potion is destroyed
    }

    private void FixedUpdate()
    {
        //potion is to ignore collisions with player so they will phase through
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), true); 
    }

    // Update is called once per frame
    void Update()
    {
        throwForce = playerScript.throwCharge; //update force as charge increases
        Vector3 throwVec = playerScript.potionVel * throwForce; //multiply potionVel by throwforce fo raccurate direction and force
        if (applyForce)
        {
            //apply force, "throwing" the potion
            rb.AddForce(throwVec, ForceMode.VelocityChange);
            applyForce = false;//ensure this loop only runs once per potion
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //allow player to throw another potion and destroy current
        playerScript.canThrow = true;
        Destroy(gameObject);
    }
}
