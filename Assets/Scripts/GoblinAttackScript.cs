using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttackScript : MonoBehaviour
{
    public Transform target;
    public float throwingAngle;
    public float throwingDelay;
    public float throwingForce;
    //Offset to the y-axis
    public float throwOffset;
    public float artificalGravity;

    public Transform projectile;
    private Transform myTransform;
    //Test variables
    public float elapseTime = 0;
    public GameObject potionPrefab;

    // Start is called before the first frame update
    private void Awake()
    {
        myTransform = transform;
    }
    //void Start()
    //{
    //    //StartCoroutine(SimulateProjectile());
    //}

   
    // Update is called once per frame
    void Update()
    {
        //Short delay beford the projectile is thrown
        Invoke("SpawnPotion", throwingDelay * Time.deltaTime);

        Vector3 direction = target.transform.position - myTransform.position;

        potionPrefab.GetComponent<Rigidbody>().AddForce(direction.normalized * throwingForce, ForceMode.Impulse);
        
    }

    void SpawnPotion()
    {
        Debug.Log("Invoking the spawn potion function");
        GameObject spawnPotion = Instantiate(potionPrefab);
    }
}
