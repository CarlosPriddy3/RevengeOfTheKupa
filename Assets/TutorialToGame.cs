using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialToGame : MonoBehaviour {

	private Animator fader;
	private PauseButton pauser;
	private bool is_paused;

	// Use this for initialization
	void Start () {
		fader = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<Animator>();
		pauser =  GameObject.FindGameObjectWithTag("Pauser").GetComponent<PauseButton>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (pauser.is_paused)
		{
			is_paused = true;
		}
		else
		{
			if (is_paused)
			{
				is_paused = false;
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.Return))
				{
		            fader.SetTrigger("start");
		        }
			}
		}
	}
}
