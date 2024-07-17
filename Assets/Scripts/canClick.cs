using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canClick : StateMachineBehaviour
{
     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("Katana_export").GetComponent<SwordSwinger>().cAttack = true;
        SwordSwinger swordSwinger = animator.GetComponent<SwordSwinger>();
        swordSwinger.sAnim.SetBool("swordSwinging", false);
        swordSwinger.sAnim.SetBool("heavySwordSwinging",false);
        swordSwinger.sAnim.SetBool("swordIsBlocking",false);
        swordSwinger.sAnim.SetBool("swordAgain",true);
        swordSwinger.isSwinging = false;
  
    }
}
