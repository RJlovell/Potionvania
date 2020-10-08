using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform targetPos;

    public float arcHeight;
    public float projectileSpeed;
    Vector3 startPos;
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
        float x1 = targetPos.position.x;
        float distance = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, projectileSpeed * Time.deltaTime);
        float baseY = Mathf.Lerp(startPos.y, targetPos.position.y, (nextX - x0) / distance);
        float arc = arcHeight * (nextX - x0) * (nextX - x1)/ (-0.25f * distance * distance);
        nextPos = new Vector3(nextX, baseY + arc, transform.position.z);

        //Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPos.position, projectileSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(nextPos - transform.position);
        //transform.rotation = LookAt(nextPos - transform.position);
        transform.position = nextPos;

        if(nextPos == targetPos.position)
        {
            Destroy(gameObject);
            Debug.Log("Damag Dealt");
        }
    }

    static Quaternion LookAt(Vector3 forward)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);
    }
}
