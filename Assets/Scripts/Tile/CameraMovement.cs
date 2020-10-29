using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //An external game object
    public GameObject gameObjectToFollow;
    public float cameraSpeed;
    // Update is called once per frame
    void Update()
    {
        //The interpolation of the cameras speed in following the game object in "real time"
        float interpolation = cameraSpeed * Time.deltaTime;
        //Makes cameraPosition == to the transform of the gameObject this script is applied to
        Vector3 cameraPosition = this.transform.position;
        //Finds the value between this game objects x & y position and the x & y position of the game object,
        //that is being followed using the interpolation variable.
        cameraPosition.x = Mathf.Lerp(this.transform.position.x, gameObjectToFollow.transform.position.x, interpolation);
        cameraPosition.y = Mathf.Lerp(this.transform.position.y, gameObjectToFollow.transform.position.y, interpolation);
        //Makes this gameobject == to the value found in the previous equations
        this.transform.position = cameraPosition;
    }
}
