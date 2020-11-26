using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounding : MonoBehaviour
{
    RaycastHit hitInfo;
    Ray groundDetectRay;
    Player playerScript;
    public float maxRayDist = 1;
    Vector3 rayCastHeight;
    float timeSinceLanding = 0;
    float landedTime = 0.3f;
    Animator anim; // animator reference
    // Start is called before the first frame update
    void Start()
    {
        rayCastHeight = new Vector3(0, 1, 0);
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.landed)
        {
            if (timeSinceLanding < landedTime)
                timeSinceLanding += Time.deltaTime;
            else
                playerScript.landed = false;
        }
        groundDetectRay = new Ray(transform.position + rayCastHeight, Vector3.down);

        if (Physics.Raycast(groundDetectRay, out hitInfo, maxRayDist))
        {
            playerScript.grounded = true;
            playerScript.landed = true;
            anim.SetBool("Landing",true);
        }
        else
        {
            playerScript.grounded = false;
            anim.SetBool("Landing", false);
        }
    }
}
