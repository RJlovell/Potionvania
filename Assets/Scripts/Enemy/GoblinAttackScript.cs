using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GoblinAttackScript : MonoBehaviour
{
    public Transform target;
    //public float throwingAngle;
    public float throwingDelay;
    public float throwingForce;
    //Offset to the y-axis
    //public float throwOffset;
    //public float artificalGravity;
    public float timer = 0;
    public Transform projectile;
    private Transform myTransform;
    //Test variables
    public GameObject potionPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        myTransform = transform;
        //projectile = transform;
        //projectile = myTransform;
    }
   
    // Update is called once per frame
    void Update()
    {
        //if (throwingDelay == timer)
        //{
        //    SpawnPotion();
        //    timer = 0;
        //}
        ////Short delay beford the projectile is thrown
        ////Invoke("SpawnPotion", throwingDelay * Time.deltaTime);
        ////Vector3 direction = target.transform.position - myTransform.position;
        ////potionPrefab.GetComponent<Rigidbody>().AddForce(direction.normalized * throwingForce, ForceMode.Impulse);
        //Vector3 nextPos = Vector3.MoveTowards(projectile.position, target.position, throwingForce * Time.deltaTime);
        //projectile.rotation = Quaternion.LookRotation(nextPos - transform.position);
        //projectile.position = nextPos;
        //
        //if (throwingDelay == timer)
        //{
        //    projectile.position = nextPos;
        //    timer = 0;
        //}
        //timer++;
        //
        //if (nextPos == target.position)
        //{
        //    Arrived();
        //}
    }

    //void Arrived()
    //{
    //    Debug.Log("Damage happens");
    //}
    //void SpawnPotion()
    //{
    //    Debug.Log("Spawning the potion");
    //    GameObject spawnPotion = Instantiate(potionPrefab);
    //    spawnPotion.gameObject.transform.position = myTransform.position;
    //}
}
