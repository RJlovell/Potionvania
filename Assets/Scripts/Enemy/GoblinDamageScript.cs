using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDamageScript : MonoBehaviour
{
    public float explosionDamage;
    public float explosionRadius;
    //Vector3 explosionPos;

    public GameObject explosionEffect;

    //private void Start()
    //{
    //    explosionPos = transform.position;    
    //}
    // Update is called once per frame
    void Update()
    {
        Explode();
    }

    void Explode()
    {
        //Show effect when bomb is meant to explode
        //Instantiate(explosionEffect, transform.position, transform.rotation);

        //Get nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider nearbyObjects in colliders)
        {
            Rigidbody rb = nearbyObjects.GetComponent<Rigidbody>();
            if(rb != null)
            {
                //Add force
                //Damage
                Debug.Log(explosionDamage + " Damage has been dealt");
            }
        }

        //Remove Bomb
        Destroy(gameObject);
    }
}
