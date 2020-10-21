using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject potion;
    Rigidbody rb;
    public float impulseSpeed = 0.2f;
    public float maxSpeed = 5.0f;
    bool grounded;
    public float jumpForce = 1.0f;
    private float jumpCount;
    public float jumpTime =0.1f;
    bool jumping;
    Vector3 jumpVec;
    Vector3 mousePos;
    public float armLength = 1.0f;
    public float height = 2.4f;
    public Vector3 potionVel;
    Vector3 potionPos;
    float launchAngle;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ///firing potion
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector3 rawMousePos = Input.mousePosition;
            rawMousePos.z = 12;
            mousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

            launchAngle = Mathf.Rad2Deg * Mathf.Atan((mousePos.x - transform.position.x) / (mousePos.y - transform.position.y));
            /*if (mousePos.y < transform.position.y && launchAngle > -15 && launchAngle < 15)
            {
                
            }*/

            potionPos = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0);
            
            float mag = Mathf.Sqrt((potionPos.x * potionPos.x) + (potionPos.y * potionPos.y));
            potionPos.x /= mag;
            potionPos.y /= mag;

            ///Boundary setting for potion throwing
           /* if (mousePos.y < transform.position.y)
            {
                if (potionPos.x < 0.7 && potionPos.x > 0)
                    potionPos.x = 0.7f;
                if (potionPos.x > -0.7 && potionPos.x < 0)
                    potionPos.x = -0.7f;
                if (potionPos.y < -0.7)
                    potionPos.y = -0.7f;
            }*/
            potionVel = potionPos;

            Debug.Log($"Normalised vec: {potionPos}");
            Debug.Log($"Angle: {launchAngle}");
            potionPos.x += transform.position.x;
            potionPos.y += transform.position.y + (height/2);
            potionPos.x *= armLength;
            potionPos.y *= armLength;
            
            

            Instantiate(potion, potionPos, transform.rotation);
        }


        //right movement using AddForce for smoother movement along X-axis
        if (Input.GetKey(KeyCode.D)) 
        {
            /*gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                gameObject.transform.eulerAngles.y + 180,
                gameObject.transform.eulerAngles.z);*/
            transform.rotation = Quaternion.Euler(0, 90, 0);

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
            transform.rotation = Quaternion.Euler(0, -90, 0);
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
            //Debug.Log("On the Ground");
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            grounded = false;
            //Debug.Log("In the air");
        }
    }
}
