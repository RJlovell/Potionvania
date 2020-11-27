using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    PlayerHealth playerHPScript;
    GoblinScript goblin;
    //Requires an external game object
    Transform projectileTargetPos;
    //Determines the height of the arc being thrown
    public float arcHeight;
    //Speed of the projectile
    public float projectileSpeed;
    //Until changes, this is the start position for the game object that has this script attached
    Vector3 startPos;
    //The position that the projectile will move to next
    Vector3 nextPos;
    // Start is called before the first frame update
    public GameObject impact;
    //particel effect object
    float particleTimer;
    //particle effect object lifetime value
    void Start()
    {
        startPos = transform.position;
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        goblin = GameObject.FindGameObjectWithTag("Goblin").GetComponent<GoblinScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //Compute the next position, with arc added in
        float x0 = startPos.x;
        float x1 = projectileTargetPos.position.x;
        //The distance between the target position and the start position
        float distance = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, projectileSpeed * Time.deltaTime);
        float baseY = Mathf.Lerp(startPos.y, projectileTargetPos.position.y, (nextX - x0) / distance);
        float arc = arcHeight * (nextX - x0) * (nextX - x1)/ (-0.25f * distance * distance);
        nextPos = new Vector3(nextX, baseY + arc, transform.position.z);
        //So the object will rotate within the direction it is meant to be moving in.
        transform.position = nextPos;

        if (goblin.deathTriggered)
            Destroy(gameObject);
        if (particleTimer >= 1f)
        {
            particleTimer -= Time.deltaTime;//countdown particle object life span if active
        }
    }

    public void SetTarget(Transform targetPos)
    {
        projectileTargetPos = targetPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(impact,projectileTargetPos); //instantiate particle effect
        particleTimer = 1f; //set countdown for particle object life span
        if(particleTimer <= 0f)
        {
            Destroy(impact);//destroy particle after 1 second
        }
        Destroy(gameObject);
        if (collision.gameObject.CompareTag("Player") && !playerHPScript.iSceneEnabled)
        {
            playerHPScript.iSceneEnabled = true;
            playerHPScript.TakeDamage(goblin.goblinDamage);
        }
        
    }
}
