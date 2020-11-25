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
    public float attackCountdown;
    //Offset to the y-axis
    //public float throwOffset;
    //public float artificalGravity;
    public bool hasThrown = false;
    private Transform potionTransform;
    private Transform myTransform;
    //Test variables
    public GameObject bombPrefab;
    GameObject potionSpawn;
    public float bombSpawnXOffset;
    public float bombSpawnYOffset;
    Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        myTransform = transform;
        attackCountdown = attackDelay;
        potionTransform = bombPrefab.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasThrown)
        {
            attackCountdown -= Time.deltaTime;
        }
        else
        {
            hasThrown = false;
        }
        //attackCountdown -= Time.deltaTime;
        if (attackCountdown <= 0 && !hasThrown)
        {
            anim.SetTrigger("throw");
            SpawnPotion();
            hasThrown = true;
            attackCountdown = attackDelay;
            
        }
    }
    void SpawnPotion()
    {
        
        Debug.Log("Spawning the potion");
        potionSpawn = Instantiate(bombPrefab);
        potionSpawn.gameObject.transform.position = new Vector3(myTransform.position.x + bombSpawnXOffset, myTransform.position.y + bombSpawnYOffset, myTransform.position.z);
        potionSpawn.GetComponent<Projectile>().SetTarget(targetPosition);
        
    }
    private void OnDestroy()
    {
        Destroy(potionPrefab);
    }
}
