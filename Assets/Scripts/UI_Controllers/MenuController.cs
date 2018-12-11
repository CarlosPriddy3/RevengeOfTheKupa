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
	public GameObject credits_menu;

	private PauseButton pauser;
	private StartMenuController starter;
    private SoundManager soundManager;
	private CheckpointRestart restarter;

	// Get pausing component if it exists.
	void Start()
	{
		pauser = GetComponent<PauseButton>();
		starter = GetComponent<StartMenuController>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
		GameObject restart_object = GameObject.FindGameObjectWithTag("CheckpointRestart");
		if (restart_object != null)
		{
			restarter = restart_object.GetComponent<CheckpointRestart>();
		}
	}

	/// <summary>
	/// Starts game, only from start menu.
	/// </summary>
	public void startGame()
	{
        //soundManager.PlayGameMusic();
        SelectedStart.isTutorial = false;
		SceneManager.LoadScene(intro_scene_name);
	}

	/// <summary>
	/// Starts the actual gameplay.
	/// </summary>
	public void play(bool isRestart)
	{
		if (pauser != null)
		{
			pauser.unpause();
		}
        if (!isRestart)
        {
            soundManager.PlayGameMusic();
        }
		GameState.state = State.PLAY;
		SceneManager.LoadScene(play_scene_name);
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
 	    Time.timeScale = 1f;
        soundManager.PlayMenuMusic(false);
		GameState.state = State.START;
		SceneManager.LoadScene(title_scene_name);
	}

	/// <summary>
	/// Directly starts the tutorial
	/// </summary>
	public void startTutorial()
	{
		if (pauser != null)
		{
			pauser.unpause();
		}
        soundManager.PlayGameMusic();
		SceneManager.LoadScene(tutorial_scene_name);
	}

    /// <summary>
    /// Takes to the tutorial scene.
    /// </summary>
    public void toTutorial()
    {
 	    Time.timeScale = 1f;
		SelectedStart.isTutorial = true;
        SceneManager.LoadScene(intro_scene_name);
    }

    /// <summary>
    /// Takes to the game loss screen.
    /// </summary>
    public void loseGame()
    {
        soundManager.PlayDefeatMusic();
        GameState.state = State.LOSS;
        SceneManager.LoadScene(end_scene_name);
    }

	/// <summary>
	/// Go to the game win screen.
	/// </summary>
	public void winGame()
	{
        soundManager.PlayVictoryMusic();
		GameState.state = State.WIN;
		SceneManager.LoadScene(end_scene_name);
	}

	/// <summary>
	/// Restarts the game from the last checkpoint.
	/// </summary>
	public void startFromCheckpoint()
	{
		if (restarter != null)
		{
			Time.timeScale = 1f;
			soundManager.PlayGameMusic();
			GameState.state = State.PLAY;
			restarter.restart();
		}
		else
		{
			play(true);
		}
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
		if (credits_menu != null)
		{
			credits_menu.SetActive(false);
		}
	}

	/// <summary>
	/// Moves to a different screen to view the credits.
	/// </summary>
	public void viewCredits()
	{
		if (starter != null)
		{
			starter.setViewingControls(true);
		}
		standard_menu.SetActive(false);
		credits_menu.SetActive(true);
	}

	/// <summary>
	/// Exits the game.
	/// </summary>
	public void exitGame()
	{
		Application.Quit();
	}
}
