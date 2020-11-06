using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    GameCondition gameCon;

    private void Start()
    {
        gameCon = GameObject.FindGameObjectWithTag("Player").GetComponent<GameCondition>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Win game
            Debug.Log("You win");
            gameObject.SetActive(false);
            gameCon.Victory();
        }
    }
}
