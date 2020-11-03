using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject potion;
    Rigidbody rb;
    public float groundSpeed = 5.0f;
    float currentSpeed = 0;
    public float airSpeed = 2.5f;
    float angleFacing = 90;
    bool moving = false;
    public float rampSpeed = 2;


    public float groundFriction = 0.6f;
    bool grounded;
    public float jumpForce = 1.0f;
    private float jumpCount;
    public float jumpTime = 0.2f;
    bool jumping;
    Vector3 jumpVec;

    [System.NonSerialized]
    public Vector3 potionVel;
    Vector3 mousePos;
    public float height = 2.4f;
    Vector3 potionPos;
    float launchAngle;
    bool canThrow = true;
    public float throwDelay = 3;
    float timeSinceThrow = 0;

    private bool potionCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.velocity = Vector3.zero;
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, angleFacing, 0);
        //check if input moving, allows airPotion to move player along x axis where necessary
        if (!moving)
            currentSpeed = rb.velocity.x;

        rb.velocity = new Vector3(currentSpeed, rb.velocity.y, 0);
        //Debug.Log(rb.velocity.x);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moving = true;
            if (grounded)
                currentSpeed = -groundSpeed;
            else
            {
                if (rb.velocity.x >= 0)
                {
                    currentSpeed = -airSpeed;
                }
                else if (rb.velocity.x < -airSpeed)
                {
                    currentSpeed += Time.deltaTime * rampSpeed;
                }
                else if (rb.velocity.x > -airSpeed)
                {
                    currentSpeed -= Time.deltaTime * rampSpeed;
                }
            }
            angleFacing = -90;
        }
        if (Input.GetKeyUp(KeyCode.A))
            moving = false;
        if (Input.GetKey(KeyCode.D))
        {
            moving = true;
            if (grounded)
                currentSpeed = groundSpeed;
            else
            {
                if(rb.velocity.x <= 0)
                {
                    currentSpeed = airSpeed;
                }
                else if(rb.velocity.x > airSpeed)
                {
                    currentSpeed -= Time.deltaTime * rampSpeed;
                }
                else if (rb.velocity.x < airSpeed)
                {
                    currentSpeed += Time.deltaTime * rampSpeed;
                }
            }
            angleFacing = 90;
        }
        if (Input.GetKeyUp(KeyCode.D))
            moving = false;
        


        ///Halt player if no movement input detected or left and right input both read simultaniously
        if ((Input.GetKeyUp(KeyCode.D) && grounded) || (Input.GetKeyUp(KeyCode.A) && grounded) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            currentSpeed = 0;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            //Debug.Log("HALT");
        }

        if(!canThrow)
        {
            if(timeSinceThrow < throwDelay)
                timeSinceThrow += Time.deltaTime;
            else
            {
                canThrow = true;
                timeSinceThrow = 0;
            }
        }
        ///firing potion
        if (Input.GetKeyUp(KeyCode.Mouse0) && canThrow)
        {
            canThrow = false;
            Vector3 rawMousePos = Input.mousePosition;
            rawMousePos.z = 12;
            mousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

            //launchAngle = Mathf.Rad2Deg * Mathf.Atan((mousePos.x - transform.position.x) / (mousePos.y - transform.position.y));


            //ensures the calculation for angle of potion thrown is calculated from centre of player rather than feet.
            float yPos = transform.position.y + (height / 2); 

            potionPos = new Vector3(mousePos.x - transform.position.x, mousePos.y - yPos, 0);
            
            float mag = Mathf.Sqrt((potionPos.x * potionPos.x) + (potionPos.y * potionPos.y));
            potionPos.x /= mag;
            potionPos.y /= mag;
            potionVel = potionPos;

            //Debug.Log($"Normalised vec: {potionPos}");
            //Debug.Log($"Angle: {launchAngle}");
            potionPos.x += transform.position.x;
            potionPos.y += transform.position.y + (height/2);

            Instantiate(potion, potionPos, transform.rotation);
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
        //check that the player is on top of the platform
        float blockHeight = other.transform.localScale.y;
        float blockWidth = other.transform.localScale.x;
        float blockPosY = other.transform.position.y;
        float blockPosX = other.transform.position.x;

        if ((rb.position.y >= (blockPosY + (blockHeight / 4))) && (rb.position.x > blockPosX - blockWidth / 2) && (rb.position.x < blockPosX + blockWidth / 2))
        {
            currentSpeed = 0;
            grounded = true;
            Debug.Log("On the Ground");
            GetComponent<Collider>().material.dynamicFriction = groundFriction;
            GetComponent<Collider>().material.staticFriction = groundFriction;
        }

        //old code here//
        /*if (other.gameObject.CompareTag("Ground"))
        {
            if (rb.position.y >= blockPosY + (blockHeight / 2))
            {
                Debug.Log("Height: " + blockHeight);
                Debug.Log("Y pos: " + blockPosY);
                currentSpeed = 0;
                grounded = true;
                Debug.Log("On the Ground");
            }
        }*/
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
            Debug.Log("In the Air");
            GetComponent<Collider>().material.dynamicFriction = 0;
            GetComponent<Collider>().material.staticFriction = 0;
        }
    }
}