using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    OrcPatrolSensor orcSensor;
    public GameObject potion;
    Rigidbody rb;
    AirPotion airPotion;
    public float groundSpeed = 5.0f;
    public float airSpeed = 2.5f;
    public float rampSpeed = 2;
    float currentSpeed = 0;
    float angleFacing = 180;
    int moveDir = 0;
    [System.NonSerialized]
    public bool potionLaunch = false;
    public float maxVelocityX = 5;

    [System.NonSerialized]
    public float groundFriction = 0.6f;
    public float jumpForce = 1.0f;
    public float jumpWaitTime = 0.25f;
    float jumpCount = 0;
    Vector3 jumpVec;
    bool jumping;
    [System.NonSerialized]
    public bool landed;
    public bool grounded = false;


    [System.NonSerialized]
    public Vector3 potionVel;
    [System.NonSerialized]
    public bool canThrow = true;
    public float throwXPos = 0;
    public float throwYPos = 0;

    public bool potionExists;
    Vector3 mousePos;
    public float height = 2.4f;
    Vector3 potionPos;
    public float throwDelay = 1;
    public float timeSinceThrow = 0;
    public float throwCharge = 0;
    public float chargeSpeed = 1;
    public float minThrowForce = 1;
    public float maxThrowForce = 7;
    float timeSinceMove = 0;
    float stunDelay = 0.2f;
    public Vector3 velocityChange;


    void Start()
    {
        throwCharge = minThrowForce;

        rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;

        airPotion = GameObject.FindGameObjectWithTag("Player").GetComponent<AirPotion>();

        velocityChange = rb.velocity;

        orcSensor = GameObject.FindGameObjectWithTag("Orc").GetComponent<OrcPatrolSensor>();
    }
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, angleFacing, 0);

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

        if (!potionLaunch)
            rb.velocity = new Vector3(currentSpeed, rb.velocity.y, 0);
        velocityChange = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rawMousePos = Input.mousePosition;
        rawMousePos.z = 12;
        mousePos = Camera.main.ScreenToWorldPoint(rawMousePos);

        if(jumping && jumpCount < jumpWaitTime)
        {
            jumpCount += Time.deltaTime;
        }

        if (potionLaunch && grounded && rb.velocity == Vector3.zero && timeSinceMove < stunDelay)
        {
            timeSinceMove += Time.deltaTime;
        }
        if (timeSinceMove >= stunDelay)
        {
            potionLaunch = false;
            timeSinceMove = 0;
        }
        if ((rb.velocity.x > 0 && rb.velocity.x < airSpeed && potionLaunch && !grounded) || (rb.velocity.x < 0 && rb.velocity.x > -airSpeed && potionLaunch && !grounded))//allow air control
        {
            potionLaunch = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
            GetComponent<Collider>().material.dynamicFriction = 0; //changes to friction based on movement allows character to jump when walking into a wall
            GetComponent<Collider>().material.staticFriction = 0;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
            GetComponent<Collider>().material.dynamicFriction = 0;
            GetComponent<Collider>().material.staticFriction = 0;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            moveDir = 0;
            GetComponent<Collider>().material.dynamicFriction = 0.6f;
            GetComponent<Collider>().material.staticFriction = 0.6f;
            if (grounded)
            {
                GetComponent<Collider>().material.dynamicFriction = 0.6f;
                GetComponent<Collider>().material.staticFriction = 0.6f;
            }
        }

        ///Halt player if no movement input detected or left and right input both read simultaniously
        if ((Input.GetKeyUp(KeyCode.D) && grounded) || (Input.GetKeyUp(KeyCode.A) && grounded) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            moveDir = 3;//Halt player until input read or force applied
            currentSpeed = 0;
            //Debug.Log("HALT");
        }
        ///Linked this with the gameIsPaused bool, so the player will not spawn a potion when the game is meant to be paused
        if (!PauseMenu.gameIsPaused)
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                if (mousePos.x < transform.position.x)
                    angleFacing = -90;
                if (mousePos.x > transform.position.x)
                    angleFacing = 90;
            }
            ///charging potion throw
            if (Input.GetKey(KeyCode.Mouse0) && canThrow && throwCharge < maxThrowForce)
            {
                throwCharge += Time.deltaTime * chargeSpeed;
                //Debug.Log("We Chargin'");
            }
            ///throwing potion
            if (Input.GetKeyUp(KeyCode.Mouse0) && canThrow)
            {
                canThrow = false;

                //ensures the calculation for angle of potion thrown is calculated from centre of player rather than feet.
                float yPos = transform.position.y + (height / 2);

                potionPos = new Vector3(mousePos.x - transform.position.x, mousePos.y - yPos, 0);

                float mag = Mathf.Sqrt((potionPos.x * potionPos.x) + (potionPos.y * potionPos.y));
                potionPos.x /= mag;
                potionPos.y /= mag;
                potionVel = potionPos;
                if(transform.position.x < mousePos.x)
                    potionPos.x+= transform.position.x - throwXPos;
                else
                    potionPos.x = transform.position.x + throwXPos;
                potionPos.y = transform.position.y + throwYPos;

                Instantiate(potion, potionPos, transform.rotation);
                //throwCharge = minThrowForce;
            }
        }

        if (Input.GetKey(KeyCode.Space) && grounded && jumping == false)
        {
            Jump();
            jumping = true;
        }
    }

    public void Jump()
    {
        jumpVec = new Vector3(rb.velocity.x, jumpForce, 0);
        rb.velocity = jumpVec;
    }

    void OnCollisionEnter(Collision other)
    {
        jumping = false;
        if (potionLaunch)
        {
            potionLaunch = false;
            moveDir = 0;
        }
        if(grounded)
        {
            currentSpeed = 0;
        }
    }
}