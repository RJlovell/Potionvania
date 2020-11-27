using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GoblinAttackScript : MonoBehaviour
{
    public Transform targetPosition;
    //public float throwingAngle;
    public float attackDelay;
    public float throwingForce;
    private float attackCountdown;

    private float animThrowCountdown;
    public float animThrowDelay;
    public bool animTrigger = false;
    //Offset to the y-axis
    //public float throwOffset;
    //public float artificalGravity;
    public bool hasThrown = false;
    private Transform potionTransform;
    private Transform myTransform;
    //Test variables
    public GameObject bombPrefab;
    GameObject bombSpawn;
    public float bombSpawnXOffset;
    public float bombSpawnYOffset;
    Animator anim;
    GoblinScript goblin;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        myTransform = transform;
        attackCountdown = attackDelay;
        potionTransform = bombPrefab.transform;
        animThrowCountdown = animThrowDelay;
        
        ///Goblin = GameObject.FindGameObjectWithTag("Goblin").GetComponent<GoblinScript>() is inappropriate in this context
        ///because the Goblins will find **any** object with this tag in the scene. Using this caused the goblins to continue
        ///throwing bombs even after it died since other Goblins were alive.
        goblin = gameObject.GetComponent<GoblinScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!hasThrown)
        {
            attackCountdown -= Time.deltaTime;
        }
        else
        {
            hasThrown = false;
        }
        ///When the attack countdown reaches 0, the throw animation will play and
        ///the Goblin will be unable to throw anymore.
        ///The animTrigger will become true.
        if (attackCountdown <= 0 && !hasThrown)
        {
            anim.SetTrigger("throw");
            hasThrown = true;
            attackCountdown = attackDelay;
            animTrigger = true;

        }
        ///When this is true, it's purposeis  to delay the bomb prefab from instantiating before the throwing animation finishes
        if (animTrigger)
        {
            animThrowCountdown -= Time.deltaTime;
            if (animThrowCountdown <= 0)
            {
                SpawnPotion();
                animThrowCountdown = animThrowDelay;
                animTrigger = false;
            }
        }

    }
    void SpawnPotion()
    {
        if (!goblin.deathTriggered && !goblin.offGround)
        {
            bombSpawn = Instantiate(bombPrefab);
            bombSpawn.gameObject.transform.position = new Vector3(myTransform.position.x + bombSpawnXOffset, myTransform.position.y + bombSpawnYOffset, myTransform.position.z);
            bombSpawn.GetComponent<Projectile>().SetTarget(targetPosition);
        }
    }
}
