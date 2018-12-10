using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For managing things on the start menu.
/// </summary>
public class StartMenuController : MonoBehaviour
{
	private MenuController controller;
	private bool is_controls;
	private bool is_keydown;

	// Use this for initialization
	void Start ()
	{
		controller = GetComponent<MenuController>();
		is_controls = false;
		is_keydown = false;
		controller.viewStandard();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.anyKeyDown)
		{
			if (!is_keydown && (Input.GetButton("Jump") || Input.GetButton("Submit")))
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

	public void setViewingControls(bool is_viewing)
	{
		is_controls = is_viewing;
		is_keydown = is_viewing;
	}
}
