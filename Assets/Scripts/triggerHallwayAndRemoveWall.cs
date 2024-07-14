using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerHallwayAndRemoveWall : MonoBehaviour
{
    public GameObject wallToRemove;
    public GameObject GScharacter;
    public GameObject hallway;
    void OnTriggerEnter(){
        GScharacter.GetComponent<Animator>().SetBool("triggerCutscene",true);
        wallToRemove.SetActive(false);
        hallway.SetActive(true);
    }
}
