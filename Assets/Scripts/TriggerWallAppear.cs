using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWallAppear : MonoBehaviour
{
    public GameObject wall;
    public GameObject hallway;

    void OnTriggerEnter(){
        wall.SetActive(true);
        hallway.SetActive(false);
    }
}
