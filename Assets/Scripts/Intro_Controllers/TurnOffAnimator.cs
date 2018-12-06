using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviors for the cutscene animator to conduct.
/// </summary>
public class TurnOffAnimator : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		VisibilityController controller = animator.GetComponentInParent<VisibilityController>();
		if (stateInfo.IsName("Fade Out"))
		{
			controller.hideIcon();
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.SetBool("fading", false);
		animator.ResetTrigger("skipToBlank");
		animator.ResetTrigger("skipToVisible");
		VisibilityController controller = animator.GetComponentInParent<VisibilityController>();
		if (stateInfo.IsName("Fade In"))
		{
			controller.showIcon();
		}
	}
}
