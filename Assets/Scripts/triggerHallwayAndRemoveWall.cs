using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerHallwayAndRemoveWall : MonoBehaviour
{
    public GameObject wallToRemove;

    public GameObject hallway;
    void OnTriggerEnter(){
        wallToRemove.SetActive(false);
        hallway.SetActive(true);
    }
}
