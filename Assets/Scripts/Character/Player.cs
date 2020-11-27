using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject potion;
    Rigidbody rb;
    Aiming aim;

    public float groundSpeed = 5.0f;
    public float airSpeed = 2.5f;
    public float rampSpeed = 2;
    float currentSpeed = 0;
    float angleFacing = 180;
    public float turnSpeed = 10;
    bool right;
    bool left;
    [System.NonSerialized]
    public int moveDir = 0;
    [System.NonSerialized]
    public bool potionLaunch = false;
    public float maxVelocityX = 5;

    [System.NonSerialized]
    public float groundFriction = 0.6f;
    public float jumpForce = 1.0f;
    public float jumpWaitTime = 0.25f;
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
    bool held = false;

    public bool potionExists;
    Vector3 mousePos;
    public float height = 2.4f;
    Vector3 potionPos;
    public float throwDelay = 1;
    public float timeSinceThrow = 0;
    public float throwCharge = 0;
    public float chargeSpeed = 1;
    public float timeHeld = 0;
    public float timeBeforeAimAssistance = 0.5f;
    public float minThrowForce = 1;
    public float maxThrowForce = 7;
    float timeSinceMove = 0;
    float stunDelay = 0.2f;
    public Vector3 velocityChange;
    Animator anim; //animator reference
    AudioSource sound;


    public bool dead = false;

    void Start() ///initialise start game variables and attach links to other scripts needed for interaction
    {
        throwCharge = minThrowForce;

        rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;

        velocityChange = rb.velocity;

        aim = GameObject.FindGameObjectWithTag("Arrow").GetComponent<Aiming>();

        aim.gameObject.SetActive(false);

        anim = GetComponent<Animator>(); //get the animator component

        sound = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        ///change angleFacing with speed to turn character around smoothly and quickly
        transform.rotation = Quaternion.Euler(0, angleFacing, 0);
        if(right && angleFacing > 90)
        {
            angleFacing -= turnSpeed * Time.deltaTime;
        }
        if(right && angleFacing <= 90)
        {
            angleFacing = 90;
            right = false;
        }
        if (left && angleFacing < 270)
        {
            angleFacing += turnSpeed * Time.deltaTime;
        }
        if(left && angleFacing >= 270)
        {
            angleFacing = 270;
            left = false;
        }
        ///check for move Direction as long as player is alive
        if (moveDir == 1 && !dead)
        {
            if(!held && !left) //let direction player is facing be overwritten by the direction the player is aiming when holding aim
                right = true;

            if (!potionLaunch) //Allow control if the player is not currently being potion Launched
            {
                if (grounded) // if grounded, let the player run at the set ground speed
                    currentSpeed = groundSpeed;
                else // if the player is in the air, make movement speed slow down to set air speed for as long as the player continues in the jumped direction
                {
                    if (rb.velocity.x <= 0) // if the player turns around mid air, set speed to airspeed value
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
        else if (moveDir == -1 && !dead)//Same as above section for moveDir==1 but inverted for moving in the other direction
        {
            //angleFacing = -90;
            if(!held && !right)
                left = true;

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
        else if (moveDir == 0 || potionLaunch && !dead) // if the player is potionlaunched of moveDirection is set to 0, let the physics engine and applied forces control the player's movement if any are applied
        {
            currentSpeed = rb.velocity.x;
        }

        if (!potionLaunch && !dead) //as long as the player is not being potion launched and is alive, x axis speed is controlled by currentSpeed variable and Y axis movement continues to be controlled by the engine and forces applied.
            rb.velocity = new Vector3(currentSpeed, rb.velocity.y, 0);
        velocityChange = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)//Only check for inputs if the player is alive AKA not mid Death animation
        {
            Vector3 rawMousePos = Input.mousePosition;
            rawMousePos.z = 12;
            mousePos = Camera.main.ScreenToWorldPoint(rawMousePos); //get mouse on screen position


            if (potionLaunch && grounded && rb.velocity == Vector3.zero && timeSinceMove < stunDelay) //checked if potionlaunch caused player to never leave ground and inable to move
            {
                timeSinceMove += Time.deltaTime;
            }
            if (timeSinceMove >= stunDelay) //reEnable movement if inability to mve lasts stunDelay duration
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
                moveDir = -1; //set movement direction for FixedUpdate
                GetComponent<Collider>().material.dynamicFriction = 0; //changes to friction based on movement allows character to jump when walking into a wall
                GetComponent<Collider>().material.staticFriction = 0;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDir = 1; //set movement direction for FixedUpdate
                GetComponent<Collider>().material.dynamicFriction = 0;
                GetComponent<Collider>().material.staticFriction = 0;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                moveDir = 0; //set movement direction for FixedUpdate. 0 being not left nor right
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
                moveDir = 3; //Halt player until input read or force applied
                currentSpeed = 0;
            }
            ///Linked this with the gameIsPaused bool, so the player will not spawn a potion when the game is meant to be paused
            if (!PauseMenu.gameIsPaused)
            {
                if (Input.GetKey(KeyCode.Mouse0) && canThrow) //check for mouseclick and canThrow bool for aiming purposes
                {
                    held = true; //bool for retaining info on mouse click being held. Allows aiming to overrride the direction the player faces
                    ///check mouse position for changing Facing Direction
                    if (mousePos.x < transform.position.x)
                    {
                        right = false;
                        left = true;
                    }
                    if (mousePos.x > transform.position.x)
                    {
                        left = false;
                        right = true;
                    }
                }
                ///charging potion throw and checking time held for aim assistance
                if (Input.GetKey(KeyCode.Mouse0) && canThrow && throwCharge < maxThrowForce)
                {
                    throwCharge += Time.deltaTime * chargeSpeed;
                    if (timeHeld < 3)
                        timeHeld += Time.deltaTime;
                    if (timeHeld >= timeBeforeAimAssistance)
                        aim.gameObject.SetActive(true);
                }
                ///throwing potion on release of mouse click
                if (Input.GetKeyUp(KeyCode.Mouse0) && canThrow)
                {
                    held = false;
                    right = false;
                    left = false;
                    aim.gameObject.SetActive(false);
                    timeHeld = 0;
                    canThrow = false;

                    //ensures the calculation for angle of potion thrown is calculated from centre of player rather than feet.
                    float yPos = transform.position.y + (height / 2);

                    //get normalised vector between player and mouseclick for velocity direction of throw
                    potionPos = new Vector3(mousePos.x - transform.position.x, mousePos.y - yPos, 0);

                    float mag = Mathf.Sqrt((potionPos.x * potionPos.x) + (potionPos.y * potionPos.y));
                    potionPos.x /= mag;
                    potionPos.y /= mag;
                    potionVel = potionPos;
                    if (transform.position.x < mousePos.x)
                        potionPos.x = transform.position.x - throwXPos; //throwXPos used in engine for lining up potion spawn position
                    else
                        potionPos.x = transform.position.x + throwXPos; //throwYPos used in engine for lining up potion spawn position
                    potionPos.y = transform.position.y + throwYPos;
                    
                    Instantiate(potion, potionPos, transform.rotation); //spawn potion
                }
            }

            if (Input.GetKey(KeyCode.Space) && grounded && jumping == false) //check player is grounded and not currently jumping when space is pressed
            {
                Jump();
                sound.Play(); // play jump sound
                anim.SetTrigger("jumpStart"); //animation trigger call
                jumping = true;
            }
        }
        else // if dead, seize movement and velocity in x direction, allowing player to fall
        {
            currentSpeed = 0;
            moveDir = 3;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    public void Jump()
    {
        jumpVec = new Vector3(rb.velocity.x, jumpForce, 0);
        rb.velocity = jumpVec; //set player velocity to be it's existing x velocity, setting the Y velocity to the JumpForce chosen in engine
    }

    void OnCollisionEnter(Collision other)
    {
        jumping = false; //No longer jumping but not necessarily grounded
        if (potionLaunch)
        {
            potionLaunch = false; //deactivate potionLaunch effect when grounded
            moveDir = 0; //Deactivate movement until input is read
        }
        if(grounded)
        {
            currentSpeed = 0; //set speed to 0 until input is read
        }
    }
}