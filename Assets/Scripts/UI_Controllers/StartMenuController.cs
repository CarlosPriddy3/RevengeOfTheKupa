using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains on-click methods for the start menu buttons.
/// </summary>
public class StartMenuController : MonoBehaviour {

	public string play_scene_name;
	public string credits_scene_name;

	/// <summary>
	/// Starts the actual gameplay.
	/// </summary>
	public void play()
	{
		SceneManager.LoadSceneAsync(play_scene_name);
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
