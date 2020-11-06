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
    public float airSpeed = 2.5f;
    public float rampSpeed = 2;
    float currentSpeed = 0;
    float angleFacing = 180;
    int moveDir = 0;
    [System.NonSerialized]
    public bool potionLaunch = false;

    
    public float groundFriction = 0.6f;
    public float jumpForce = 1.0f;
    public float jumpTime = 0.2f;
    Vector3 jumpVec;
    private float jumpCount;
    bool jumping;
    bool grounded;
    

    [System.NonSerialized]
    public Vector3 potionVel;
    Vector3 mousePos;
    public float height = 2.4f;
    Vector3 potionPos;
    bool canThrow = true;
    public float throwDelay = 3;
    float timeSinceThrow = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
    }
    private void FixedUpdate()
    {
        if (moveDir == 1)
        {
            angleFacing = 90;
            if (!potionLaunch)
            {
                if (grounded)
                    currentSpeed = groundSpeed;
                else
                {
                    if (rb.velocity.x <= 0)
                    {
                        currentSpeed = airSpeed;
                    }
                    else if (rb.velocity.x > airSpeed)
                    {
                        currentSpeed -= Time.fixedDeltaTime * rampSpeed;
                    }
                    else if (rb.velocity.x < airSpeed)
                    {
                        currentSpeed += Time.fixedDeltaTime * rampSpeed;
                    }
                }
            }
            
        }
        else if (moveDir == -1)
        {
            angleFacing = -90;
            if (!potionLaunch)
            {
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
                        currentSpeed += Time.fixedDeltaTime * rampSpeed;
                    }
                    else if (rb.velocity.x > -airSpeed)
                    {
                        currentSpeed -= Time.fixedDeltaTime * rampSpeed;
                    }
                }
            }
        }
        else if (moveDir == 0 || potionLaunch)
        {
            currentSpeed = rb.velocity.x;
        }

        transform.rotation = Quaternion.Euler(0, angleFacing, 0);
        if(!potionLaunch)
            rb.velocity = new Vector3(currentSpeed, rb.velocity.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            moveDir = 0;
        }
        
        ///Halt player if no movement input detected or left and right input both read simultaniously
        if ((Input.GetKeyUp(KeyCode.D) && grounded) || (Input.GetKeyUp(KeyCode.A) && grounded) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            moveDir = 3;//Halt player until input read or force applied
            currentSpeed = 0;
            //Debug.Log("HALT");
        }

        if (!canThrow)
        {
            if (timeSinceThrow < throwDelay)
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

            potionPos.x += transform.position.x;
            potionPos.y += transform.position.y + (height / 2);

            Instantiate(potion, potionPos, transform.rotation);
        }


        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            jumping = true;
            jumpCount = jumpTime;
            Jump();
        }
        if (Input.GetKey(KeyCode.Space) && jumping)
        {
            if (jumpCount > 0)
            {
                Jump();
                jumpCount -= Time.deltaTime;
            }
            else
                jumping = false;

        }
        if (Input.GetKeyUp(KeyCode.Space))
            jumping = false;

    }

    public void Jump()
    {
        jumpVec = new Vector3(rb.velocity.x, jumpForce, 0);
        rb.velocity = jumpVec;
    }

    void OnCollisionEnter(Collision other)
    {
        potionLaunch = false;
        //check that the player is on top of the platform that they're Entering
        float blockHeight = other.transform.localScale.y;
        float blockWidth = other.transform.localScale.x;
        float blockPosY = other.transform.position.y;
        float blockPosX = other.transform.position.x;

        if ((rb.position.y >= (blockPosY + (blockHeight / 4))) && (rb.position.x > blockPosX - blockWidth / 2) && (rb.position.x < blockPosX + blockWidth / 2)) //if on top
        {
            currentSpeed = 0;
            grounded = true;
            Debug.Log("On the Ground");
            GetComponent<Collider>().material.dynamicFriction = groundFriction;
            GetComponent<Collider>().material.staticFriction = groundFriction;
        }

    }

    void OnCollisionExit(Collision other)
    {
        //check that the player is on top of the platform that they're Exiting
        float blockHeight = other.transform.localScale.y;
        float blockWidth = other.transform.localScale.x;
        float blockPosY = other.transform.position.y;
        float blockPosX = other.transform.position.x;
        if ((rb.position.y >= (blockPosY + (blockHeight / 4))) && (rb.position.x > blockPosX - blockWidth / 2) && (rb.position.x < blockPosX + blockWidth / 2)) //if on top
        {
            grounded = false;
            Debug.Log("In the Air");
            GetComponent<Collider>().material.dynamicFriction = 0;
            GetComponent<Collider>().material.staticFriction = 0;
        }
    }
}