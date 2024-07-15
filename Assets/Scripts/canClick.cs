using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canClick : StateMachineBehaviour
{
     override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        SwordSwinger swordSwinger = animator.GetComponent<SwordSwinger>();
        swordSwinger.sAnim.SetBool("swordSwinging", false);
        swordSwinger.sAnim.SetBool("heavySwordSwinging",false);

        swordSwinger.isSwinging = false;
  
    }
}
