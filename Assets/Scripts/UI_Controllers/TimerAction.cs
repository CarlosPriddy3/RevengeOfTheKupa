using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Encapsulates an action to perform once timer has reached zero.
/// </summary>
public class TimerAction : MonoBehaviour
{
	public string end_scene_name;

	/// <summary>
	/// The action to perform.
	/// </summary>
	public void onTimerEnd()
	{
		SceneManager.LoadSceneAsync(end_scene_name);
		GameState.state = State.LOSS;
	}
}
