using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcPatrol : MonoBehaviour
{
    protected PlayerHealth playerHPScript;
    public float speed;

    [System.NonSerialized]public bool movingRight;
    [System.NonSerialized]public bool dealDamage = false;

    private void Start()
    {
        playerHPScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Update()
    {

        if (movingRight)
        {
            dealDamage = false;

            //The object is moving right
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            dealDamage = false;
            //The object is moving left
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}