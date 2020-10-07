using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    bool cubeleftMovement = true;
    bool cubeCollision = false;
    public float speed;
    public float dampTime;
    float currentValue;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            speed = -speed;
            cubeCollision = true;
        }
    }

    void Update()
    {
        //currentValue = Mathf.Lerp(currentValue, speed, Time.deltaTime * dampTime);
        currentValue = speed;

        if (cubeleftMovement)
        {
            //Debug.Log("Moving left!");
            transform.position += Vector3.left * currentValue * Time.deltaTime;
            if (cubeCollision)
            {
                Debug.Log("Cube Collision has been detected and CubeLeftMoveFlip will be initiated");
                CubeLeftMoveFlip();
            }
        }
        if(!cubeleftMovement)
        {
            Debug.Log("Moving right!");
            transform.position += Vector3.right * -currentValue * Time.deltaTime;
            if(cubeCollision)
            {
                Debug.Log("Cube Collision has been detected and CubeRightMoveFlip will be initiated");
                CubeRightMoveFlip();
            }
        }
        
        
    }
    private void CubeLeftMoveFlip()
    {
        cubeleftMovement = false;
        cubeCollision = false;
    }
    private void CubeRightMoveFlip()
    {
        cubeleftMovement = true;
        cubeCollision = false;
    }

}
