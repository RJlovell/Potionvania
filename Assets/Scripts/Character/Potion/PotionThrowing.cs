using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class PotionThrowing : MonoBehaviour
{
    PotionEffect potionEffect;
    private Player playerScript;
    public GameObject player;
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
        playerScript.throwCharge = playerScript.minThrowForce;
    }

    private void FixedUpdate()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        throwForce = playerScript.throwCharge;
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
        //Debug.Log("The potion dealt " + potionEffect.potionDamage + " damage against " + other.gameObject.name);
        Destroy(gameObject);
    }
}
