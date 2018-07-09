using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateChildrenSpriteRenderers : StateMachineBehaviour
{
    SpriteRenderer thisSpriteRenderer;
    SpriteRenderer[] childrenSpriteRenderers;
    Color[] childrenColors;
    bool byPass;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        thisSpriteRenderer = animator.GetComponent<SpriteRenderer>();
        byPass = thisSpriteRenderer == null;
        if (byPass)
            return;

        childrenSpriteRenderers = animator.GetComponentsInChildren<SpriteRenderer>();
        byPass = childrenSpriteRenderers.Length < 2;
        if (byPass)
            return;

        childrenColors = new Color[childrenSpriteRenderers.Length];
        for (int i = 0; i < childrenSpriteRenderers.Length; i++)
        {
            childrenColors[i] = childrenSpriteRenderers[i].color;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (byPass)
            return;

        for (int i = 0; i < childrenSpriteRenderers.Length; i++)
        {
            if (thisSpriteRenderer != childrenSpriteRenderers[i])
                childrenSpriteRenderers[i].color = thisSpriteRenderer.color;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (byPass)
            return;

        //below code is just for being sure that any child renderer's color will be turn to its original
        //esp. when the animatoin stops before reaching its end
        for (int i = 0; i < childrenSpriteRenderers.Length; i++)
        {
            childrenSpriteRenderers[i].color = childrenColors[i];
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
