﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

	public Canvas pause_menu;
	public MenuButton first_selection;

	private MenuController controller;
	public bool is_paused;
	private bool is_controls;
	private bool is_keydown;

	private SavePointManager spManager;
	private MenuButton checkpoint_menubutton;

	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<MenuController>();
		GameObject spManagerObject = GameObject.FindGameObjectWithTag("SavePointManager");
        if (spManagerObject != null)
        {
            spManager = spManagerObject.GetComponent<SavePointManager>();
        }
		GameObject checkpoint_button = GameObject.FindGameObjectWithTag("CheckpointButton");
		checkpoint_menubutton = checkpoint_button.GetComponent<MenuButton>();
		is_paused = false;
		is_controls = false;
		is_keydown = false;
		pause_menu.enabled = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.anyKeyDown)
		{
			if (Input.GetButton("Menu"))
			{
				if (is_paused)
				{
					unpause();
				}
				else
				{
					controller.viewStandard();
					pause();
				}
			}
			else if (!is_keydown && (Input.GetButton("Jump") || Input.GetButton("Submit")))
			{
				if (is_controls)
				{
					controller.viewStandard();
					is_controls = false;
				}
			}
		}
		else
		{
			is_keydown = false;
		}
	}

	public void pause()
	{
		if (spManager != null && spManager.getSavePointPosition() == Vector3.zero)
		{
			checkpoint_menubutton.disable();
		}
		GameState.paused = true;
		pause_menu.enabled = true;
		Time.timeScale = 0f;
		is_paused = true;
		first_selection.Select();
	}

	public void unpause()
	{
		GameState.paused = false;
		pause_menu.enabled = false;
		Time.timeScale = 1f;
		is_paused = false;
		is_controls = false;
	}

	public void setViewingControls(bool is_viewing)
	{
		is_controls = is_viewing;
		is_keydown = true;
	}
}
