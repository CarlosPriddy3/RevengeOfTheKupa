using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains on-click methods for the menu buttons.
/// </summary>
public class MenuController : MonoBehaviour {

	public string intro_scene_name;
	public string title_scene_name;
	public string play_scene_name;
    public string tutorial_scene_name;
	public string end_scene_name;
	public string credits_scene_name;
	public GameObject standard_menu;
	public GameObject controls_menu;

	private PauseButton pauser;
	private StartMenuController starter;
    private SoundManager soundManager;

	// Get pausing component if it exists.
	void Start()
	{
		pauser = GetComponent<PauseButton>();
		starter = GetComponent<StartMenuController>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
	}

	/// <summary>
	/// Starts game, only from start menu.
	/// </summary>
	public void startGame()
	{
		SelectedStart.isTutorial = false;
		SceneManager.LoadSceneAsync(intro_scene_name);
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
        soundManager.PlayGameMusic();
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
        soundManager.PlayMenuMusic();
		SceneManager.LoadSceneAsync(title_scene_name);
		GameState.state = State.START;
	}

    /// <summary>
    /// Takes to the tutorial scene.
    /// </summary>
    public void toTutorial()
    {
		SelectedStart.isTutorial = true;
        SceneManager.LoadSceneAsync(intro_scene_name);
    }

    /// <summary>
    /// Takes to the game loss screen.
    /// </summary>
    public void loseGame()
    {
        soundManager.PlayDefeatMusic();
        SceneManager.LoadSceneAsync(end_scene_name);
        GameState.state = State.LOSS;
    }

	/// <summary>
	/// Go to the game win screen.
	/// </summary>
	public void winGame()
	{
        soundManager.PlayVictoryMusic();
		SceneManager.LoadSceneAsync(end_scene_name);
		GameState.state = State.WIN;
	}

	/// <summary>
	/// Displays the controls menu.
	/// </summary>
	public void viewControls()
	{
		if (pauser != null)
		{
			pauser.setViewingControls(true);
		}
		if (starter != null)
		{
			starter.setViewingControls(true);
		}
		standard_menu.SetActive(false);
		controls_menu.SetActive(true);
	}

	/// <summary>
	/// Views the standard menu.
	/// </summary>
	public void viewStandard()
	{
		standard_menu.SetActive(true);
		controls_menu.SetActive(false);
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
