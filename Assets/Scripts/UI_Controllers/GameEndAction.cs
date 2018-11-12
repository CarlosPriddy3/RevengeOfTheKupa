using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Encapsulates an action to perform when reaching a game ending state.
/// </summary>
public class GameEndAction : MonoBehaviour
{
	public string end_scene_name;

	/// <summary>
	/// The action to perform.
	/// </summary>
	public void onLoss()
	{
		SceneManager.LoadSceneAsync(end_scene_name);
		GameState.state = State.LOSS;
	}
}
