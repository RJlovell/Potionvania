using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    Player player;
    Vector3 mousePos;
    Vector3 playerScreenPos;
    RectTransform rt;
    float angle;
    // Start is called before the first frame update
    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rawMousePos = Input.mousePosition;
        rawMousePos.z = 12;
        mousePos = Camera.main.ScreenToWorldPoint(rawMousePos);
        playerScreenPos = Camera.main.WorldToScreenPoint(player.transform.position);
        playerScreenPos.y += 50;

        

        Vector3 placementVec = new Vector3(mousePos.x - player.transform.position.x, mousePos.y - (player.transform.position.y+1), 0);
        float mag = Mathf.Sqrt((placementVec.x * placementVec.x) + (placementVec.y * placementVec.y));
        placementVec.x /= mag;
        placementVec.y /= mag;
        placementVec *= 100;

        Vector3 scaleVec = Vector3.one;
        if(player.throwCharge > player.minThrowForce)
        {
            float range = player.maxThrowForce - player.minThrowForce;
            float percentage = (player.throwCharge - player.minThrowForce) / range;


            scaleVec = Vector3.one * (1 + percentage/2);

        }


        angle = Mathf.Atan((mousePos.x - player.transform.position.x) / (mousePos.y - player.transform.position.y - 1));
        angle *= 180 / Mathf.PI;
        if (mousePos.y <= player.transform.position.y + 1)
        {
            angle += 180;
        }

        rt.localScale = scaleVec;
        rt.position = playerScreenPos + placementVec;
        rt.transform.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
