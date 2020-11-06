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
    //OrcScript orcScript;


    public float throwForce = 1.0f;
    bool applyForce = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //potionEffect = GameObject.FindGameObjectWithTag("Potion").GetComponent<PotionEffect>();
        //orcScript = GameObject.FindGameObjectWithTag("Orc").GetComponent<OrcScript>();
    }

    private void FixedUpdate()
    {
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>(), true);
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

    //void OnCollisionEnter(Collision other)
    //{
    //    //Debug.Log("The potion dealt " + potionEffect.potionDamage + " damage //against " + other.gameObject.name);
    //    if (other.collider.gameObject.CompareTag("Orc") || //other.collider.gameObject.CompareTag("Goblin"))
    //    {
    //        Debug.Log("Player has dealt " + potionEffect.potionDamage + " to the " /+ /other.gameObject.name);
    //        if(CompareTag("Orc"))
    //        {
    //            //orcScript.TakeDamage(potionEffect.potionDamage);
    //        }
    //        if(CompareTag("Goblin"))
    //        {
    //
    //        }
    //    }
    //    Destroy(gameObject);
    //}
}
