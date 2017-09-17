using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//there's no monobehavior for scripts that extend the state machine
//public class ForExample : StateMachineBehavior
public class StateMachineDestroy : StateMachineBehaviour
{
    //this script overrides one of the functions of a state machine
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //destroys the game object this script is attached to on state exit
        Destroy(animator.gameObject);
    }


}
