using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinScript : MonoBehaviour
{
    
    [SerializeField] private bool onFloor = false;
    [SerializeField] private bool deathTriggered = false;
    [SerializeField] private bool offGround = false;
    public int goblinDamage = 1;
    private float deathanim = 2f;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.activeInHierarchy)
        {
            onFloor = true;
            if (offGround)
            {
                //Debug.Log("This is triggered");
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
            anim.SetTrigger("inAir");
        }
    }
    //private void Start()
    //{
    //    rb = gameObject.GetComponent<Rigidbody>();
    //}
    private void Update()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>()); //allows player to move through goblin
        //if(rb.velocity.x != 0 || rb.velocity.y != 0)
        //{
        //    offGround = true;
        //}
        if (deathTriggered)
        {

            anim.SetTrigger("dead");
            deathanim -= Time.deltaTime;
        }
        if (deathanim <= 0f)
        {
            GoblinDeath();
        }
        
        
        
    }

    private void GoblinDeath()
    {
        Debug.Log("The goblin has died");
        gameObject.SetActive(false);
       
    }
}
