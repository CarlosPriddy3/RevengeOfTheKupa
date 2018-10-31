using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains on-click methods for the menu buttons.
/// </summary>
public class MenuController : MonoBehaviour {

	public string title_scene_name;
	public string play_scene_name;
	public string end_scene_name;
	public string credits_scene_name;

	private PauseButton pauser;

	// Get pausing component if it exists.
	void Start()
	{
		pauser = GetComponent<PauseButton>();
	}

	/// <summary>
	/// Starts the actual gameplay.
	/// </summary>
	public void play()
	{
		if (pauser != null)
		{
			pauser.unpause();
		}
		SceneManager.LoadSceneAsync(play_scene_name);
		GameState.state = State.PLAY;
	}

	/// <summary>
	/// Unpauses the game.
	/// </summary>
	public void unpause()
	{
		if (pauser != null)
		{
			pauser.unpause();
		}
	}

	/// <summary>
	/// Returns to the title screen.
	/// </summary>
	public void toTitle()
	{
		SceneManager.LoadSceneAsync(title_scene_name);
		GameState.state = State.START;
	}

	/// <summary>
	/// Takes to the game loss screen.
	/// </summary>
	public void loseGame()
	{
		SceneManager.LoadSceneAsync(end_scene_name);
		GameState.state = State.LOSS;
	}

	/// <summary>
	/// Go to the game win screen.
	/// </summary>
	public void winGame()
	{
		SceneManager.LoadSceneAsync(end_scene_name);
		GameState.state = State.WIN;
	}

	/// <summary>
	/// Displays the controls.
	/// </summary>
	public void viewControls()
	{
		Debug.Log("Pressed \"Controls\" button");
	}

	/// <summary>
	/// Moves to a different screen to view the credits.
	/// </summary>
	public void viewCredits()
	{
		Debug.Log("Pressed \"Credits\" button");
	}

	/// <summary>
	/// Exits the game.
	/// </summary>
	public void exitGame()
	{
		Application.Quit();
	}
}
