using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBarFallDownScript : MonoBehaviour
{
    public Animator columnAnimator;
   void OnTriggerEnter(){
    Debug.Log("triggered");
        columnAnimator.SetBool("fall",true);
   }
}
