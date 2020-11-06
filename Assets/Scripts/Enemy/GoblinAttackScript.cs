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
    public GameObject potionPrefab;
    GameObject potionSpawn;
    public float potionSpawnXOffset;
    public float potionSpawnYOffset;

    // Start is called before the first frame update
    private void Start()
    {
        myTransform = transform;
        attackCountdown = attackDelay;
        potionTransform = potionPrefab.transform;
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
        if (attackCountdown <= 0f && !hasThrown)
        {

            SpawnPotion();
            hasThrown = true;
            attackCountdown = attackDelay;
        }
    }
    void SpawnPotion()
    {
        Debug.Log("Spawning the potion");
        potionSpawn = Instantiate(potionPrefab);
        potionSpawn.gameObject.transform.position = new Vector3(myTransform.position.x + potionSpawnXOffset, myTransform.position.y + potionSpawnYOffset, myTransform.position.z);
        potionSpawn.GetComponent<Projectile>().SetTarget(targetPosition);
    }
}
