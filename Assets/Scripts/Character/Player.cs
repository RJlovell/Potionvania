using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public float impulseSpeed = 0.2f;
    public float maxSpeed = 5.0f;
    bool grounded;
    public float jumpForce;
    private float jumpCount;
    public float jumpTime;
    bool jumping;
    Vector3 jumpVec;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //right movement using AddForce for smoother movement along X-axis
        if (Input.GetKey(KeyCode.D)) 
        {
            //transform.position += Vector3.right * Time.deltaTime * maxSpeed;
            if (rb.velocity.x > maxSpeed)
            {
                Vector3 xDir = new Vector3(1 * Time.deltaTime * impulseSpeed, 0, 0);
                rb.AddForce(xDir);
            }
            else
            {
                rb.velocity = new Vector3(maxSpeed, rb.velocity.y, 0);
            }

        }
        //left movement using AddForce for smoother movement along X-axis
        if (Input.GetKey(KeyCode.A))
        {
            //transform.position += Vector3.left * Time.deltaTime * maxSpeed;
            if (rb.velocity.x < -maxSpeed)
            { 
            Vector3 xDir = new Vector3(-1 * Time.deltaTime * impulseSpeed, 0, 0);
            rb.AddForce(xDir);            
            }
            else
            {
                rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, 0);
            }
        }

        if((Input.GetKeyUp(KeyCode.D) && grounded) || (Input.GetKeyUp(KeyCode.A) && grounded))
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jumping = true;
            jumpCount = jumpTime;
            Jump();
        }
        if(Input.GetKey(KeyCode.Space) && jumping)
        {
            if(jumpCount > 0)
            {
                Jump();
                jumpCount -= Time.deltaTime;
            }
            else
                jumping = false;
            
        }
        if(Input.GetKeyUp(KeyCode.Space))
            jumping = false;
        
    }

    public void Jump()
    {
        jumpVec = new Vector3(rb.velocity.x, jumpForce, 0);
        rb.velocity = jumpVec;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = true;
            Debug.Log("On the Ground");
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
            Debug.Log("In the air");
        }
    }
}
