using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth;

    public bool iSceneEnabled = false;
    public float iSceneDuration;
    public float iSceneCountdown;
    //ISceneScript invinciScript;

    private void Start()
    {
        iSceneCountdown = iSceneDuration;
        //invinciScript = GetComponent<ISceneScript>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Orc"))
        {
            print("Invincible script enabled");
            iSceneEnabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (iSceneEnabled)
        {
            iSceneCountdown -= Time.deltaTime;
        }
        if (iSceneCountdown <= 0f && iSceneEnabled)
        {
            iSceneEnabled = false;
            iSceneCountdown = iSceneDuration;
            print("IScene is now disabled & iScene is disabled");
        }
    }
}
