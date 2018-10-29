using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

	public Canvas pause_menu;
	public MenuButton first_selection;

	private MenuController controller;
	private bool is_paused;

	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<MenuController>();
		is_paused = false;
		pause_menu.enabled = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Menu"))
		{
			if (is_paused)
			{
				unpause();
			}
			else
			{
				pause();
			}
		}
	}

	public void pause()
	{
		pause_menu.enabled = true;
		Time.timeScale = 0f;
		is_paused = true;
		first_selection.Select();
	}

	public void unpause()
	{
		pause_menu.enabled = false;
		Time.timeScale = 1f;
		is_paused = false;
	}
}
