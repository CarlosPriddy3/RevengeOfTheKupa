using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Encapsulates an action to perform when reaching a game ending state.
/// </summary>
public class GameEndAction : MonoBehaviour
{
	private SoundManager soundManager;
	private Animator fader;

	void Start()
	{
        fader = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<Animator>();
		soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
	}

    /// <summary>
    /// The action to perform.
    /// </summary>
    public void onLoss()
	{
		soundManager.fadeOut();
		fader.SetTrigger("lose");
	}
}
