using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightDirection : MonoBehaviour
{
    public GameObject sceneLight;
    private Material m;

    void Start()
    {
        m = GetComponent<Renderer>().material;
    }

    void Update()
    {
        Vector4 tempVector = new Vector4(sceneLight.transform.rotation.eulerAngles.x / 360.0f, sceneLight.transform.rotation.eulerAngles.y / 360.0f, sceneLight.transform.rotation.eulerAngles.z / 360.0f, 0.0f);
        m.SetVector("Vector3_28DF2825", tempVector);
    }
}
