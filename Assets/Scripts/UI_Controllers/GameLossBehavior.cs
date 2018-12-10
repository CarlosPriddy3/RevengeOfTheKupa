using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLossBehavior : StateMachineBehaviour
{
	private SoundManager soundManager;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
   override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
	   soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
	   Time.timeScale = 0f;
   }

   // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   {
	   Time.timeScale = 1f;
	   soundManager.PlayDefeatMusic();
	   GameState.state = State.LOSS;
	   SceneManager.LoadScene("GameEnd");
   }
}
