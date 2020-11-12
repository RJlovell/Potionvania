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
    // Start is called before the first frame update
    void Start()
    {
        rayCastHeight = new Vector3(0, 1, 0);
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        groundDetectRay = new Ray(transform.position + rayCastHeight, Vector3.down);

        if (Physics.Raycast(groundDetectRay, out hitInfo, maxRayDist))
        {
            playerScript.grounded = true;
        }
        else
        {
            playerScript.grounded = false;
        }
    }
}
