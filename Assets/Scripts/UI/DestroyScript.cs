using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeLimit;
    
    // Update is called once per frame
    void Update()
    {
        DestroyGameObject();
    }

    void DestroyGameObject()
    {
        Destroy(gameObject, timeLimit);
    }
}
