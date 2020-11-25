using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcPatrolSensor : MonoBehaviour
{
    PlayerHealth playerHPScript;
    OrcPatrol orcPatrolParent;
    OrcScript orc;
    Player player;

    Collider orcColliderInfo;
    Collider playerColliderInfo;
    public bool moveThrough = true;


    public float xAxisForce;
    public float yAxisForce;

    public float rayCastOffset;
    public float maxRayDistance;
    RaycastHit hitInfo;
    public Ray groundDetectRay;

    Vector3 orcKnockBackVelocity;
    public float knockBackForce = 7.0f;
    float magnitude;
    Vector3 orcPosition;
    public float potionLaunchEffectHeight = 1;
    Animator anim;


    private void Start()
    {
        ///Finds all the game objects with the tags to get the components required to complete the script.
        ///
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        orcPatrolParent = GetComponentInParent<OrcPatrol>();
        orc = GetComponentInParent<OrcScript>();

        ///Stores the Information of the these gameobjects colliders
        orcColliderInfo = GameObject.FindGameObjectWithTag("Orc").gameObject.GetComponent<Collider>();
        playerColliderInfo = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Collider>();
        //potionColliderInfo = GameObject.FindGameObjectWithTag("Potion").gameObject.GetComponent<Collider>();

        ///Always ignore collision between the Orc and Player.
        Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, true);
        anim = GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        ///If the colliding object is the Player and they are not invincible this function will set the players velocity to 0
        ///and the potion launch variable to true so they player can get properly knocked back/up when the add force is called.
        ///When the player triggers/(collides with) the orc the iSceneEnabled becomes true and applys damage to the player.
        ///This is needed so the player is not constantly dealt damage before the invincibility
        if (other.CompareTag("Player") && !playerHPScript.iSceneEnabled)
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            player.potionLaunch = true;

            orcPosition = transform.position;
            orcKnockBackVelocity = new Vector3((other.attachedRigidbody.transform.position.x) - orcPosition.x, (other.attachedRigidbody.transform.position.y + potionLaunchEffectHeight) - orcPosition.y, 0);

            //normalise vector
            magnitude = AirPotion.GetMag(orcKnockBackVelocity.x, orcKnockBackVelocity.y);
            orcKnockBackVelocity.x /= magnitude;
            orcKnockBackVelocity.y /= magnitude;

            //apply explosion force
            orcKnockBackVelocity *= knockBackForce;
            //zero velocity then add force to rigid body

            other.attachedRigidbody.AddForce(orcKnockBackVelocity, ForceMode.Impulse);

            playerHPScript.iSceneEnabled = true;

            if (!orcPatrolParent.dealDamage)
            {
                playerHPScript.TakeDamage(orc.orcDamage);
                orcPatrolParent.dealDamage = true;
            }

        }
        else if (other.CompareTag("Potion"))
        {
            moveThrough = true;
            anim.SetTrigger("stun");
        }
        else
        {
            ///If the Orc is colliding with anything other then the Player and Potion,
            ///then it will turn around.
            if (!other.CompareTag("Player") && !other.CompareTag("Potion"))
            {
                orcPatrolParent.movingRight = !orcPatrolParent.movingRight;
            }
        }
    }

    private void Update()
    {
        groundDetectRay = new Ray(transform.position + new Vector3(orcPatrolParent.movingRight ? rayCastOffset : -rayCastOffset, 0, 0), Vector3.down);
        ///When the downwards raycast doesn't touch any objects/ground then the orc will determine that it has reached a ledge
        ///and turn back around.
        if (!Physics.Raycast(groundDetectRay, out hitInfo, maxRayDistance))
        {
            orcPatrolParent.movingRight = !orcPatrolParent.movingRight;
        }

        if (moveThrough)
        {
            Physics.IgnoreCollision(orcColliderInfo, playerColliderInfo, true);
        }
    }
}
