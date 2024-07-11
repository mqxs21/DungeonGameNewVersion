using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeSwordSwingingFalse : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        SwordSwinger swordSwinger = animator.GetComponent<SwordSwinger>();
        if (swordSwinger != null)
        {
     // Assuming isSwinging is the boolean controlling sword swinging

            swordSwinger.sAnim.SetBool("swordSwinging", false); // Update animator parameter if needed
            swordSwinger.isSwinging = false;
             
            
        }
        else
        {
            Debug.LogError("SwordSwinger component not found on the Animator's GameObject.");
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
