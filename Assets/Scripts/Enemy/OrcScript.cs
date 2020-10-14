using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcScript : MonoBehaviour
{
    public float orcHealth = 3;
    public float orcDamage;
    // Start is called before the first frame update
    //void Start()
    //{
    //
    //}
    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.collider.CompareTag("Player"))
        {
            print("The orc has dealt damage to the player");

        }
    }
    // Update is called once per frame
    void Update()
    {

    }

}