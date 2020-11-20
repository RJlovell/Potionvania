using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
    Player player;
    Transform platformTransform;
    public bool passThrough = false;
    Collider platformCollider;
    Collider playerCollider;

    private void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        //platformCollider = GetComponentInParent<BoxCollider>();
        platformCollider = gameObject.GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //Physics.IgnoreCollision(playerCollider, platformCollider, true);
    }

    private void Update()
    {
        if(player.transform.position.y < (transform.position.y + transform.localScale.y / 2.8))
        {
            passThrough = true;
            Physics.IgnoreCollision(playerCollider, platformCollider, true);
        }
        if(player.transform.position.y >= (transform.position.y + transform.localScale.y / 2.8))
        {
            passThrough = false;
            Physics.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }

    // Make the parent platform ignore the jumper
    private void OnTriggerEnter(Collider other)
    {
        /*if (passThrough)
        {
            //platformTransform = transform.parent;
            //Physics.IgnoreCollision(other.GetComponent<Collider>(), platformTransform.GetComponent<Collider>(), true);
            //Physics.IgnoreCollision(other.GetComponent<Collider>(), platformCollider, true);
        }*/
        /*if(other.CompareTag("Player"))
        {
            passThrough = true;
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        //passThrough = false;
        //platformTransform = transform.parent;
        //Physics.IgnoreCollision(other.GetComponent<Collider>(), platformTransform.GetComponent<Collider>(), false);
        /*if(other.CompareTag("Player"))
        {
            passThrough = false;
        }*/
    }

}
