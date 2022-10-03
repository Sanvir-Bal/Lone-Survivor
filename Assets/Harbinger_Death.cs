using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbinger_Death : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Enemy>().immune = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Enemy>().immune = false;
        if(animator.GetComponent<Enemy>().currentHealth > 0)
        {
            animator.GetComponent<Enemy>().Respawn();
        }
    }
}
