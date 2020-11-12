using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinScript : MonoBehaviour
{
    [SerializeField] private bool onFloor = false;
    [SerializeField] private bool deathTriggered = false;
    [SerializeField] private bool offGround = false;

    //Rigidbody rb;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.activeInHierarchy)
        {
            onFloor = true;
            if (offGround)
            {
                Debug.Log("This is triggered");
                deathTriggered = true;
                offGround = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (onFloor)
        {
            onFloor = false;
            offGround = true;
        }
    }
    //private void Start()
    //{
    //    rb = gameObject.GetComponent<Rigidbody>();
    //}
    private void Update()
    {
        //if(rb.velocity.x != 0 || rb.velocity.y != 0)
        //{
        //    offGround = true;
        //}
        GoblinDeath();
    }

    private void GoblinDeath()
    {
        if (deathTriggered)
        {
            Debug.Log("The goblin has died");
            gameObject.SetActive(false);
        }
    }
}
