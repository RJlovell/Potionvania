using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Requires an external game object
    public Transform projectileTargetPos;
    //Determines the height of the arc being thrown
    public float arcHeight;
    //Speed of the projectile
    public float projectileSpeed;
    //Until changes, this is the start position for the game object that has this script attached
    Vector3 startPos;
    //The position that the projectile will move to next
    Vector3 nextPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Compute the next position, with arc added in
        float x0 = startPos.x;
        float x1 = projectileTargetPos.position.x;
        //The distance between the target position and the start position
        float distance = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, projectileSpeed * Time.deltaTime);
        float baseY = Mathf.Lerp(startPos.y, projectileTargetPos.position.y, (nextX - x0) / distance);
        float arc = arcHeight * (nextX - x0) * (nextX - x1)/ (-0.25f * distance * distance);
        nextPos = new Vector3(nextX, baseY + arc, transform.position.z);
        //So the object will rotate within the direction it is meant to be moving in.
        //transform.rotation = Quaternion.LookRotation(nextPos - transform.position);
        transform.position = nextPos;
        //For debugging purposes
        if(nextPos == projectileTargetPos.position)
        {
            Debug.Log("Damage Dealt");
        }
    }

    public void SetTarget(Transform targetPos)
    {
        projectileTargetPos = targetPos;
    }
}
