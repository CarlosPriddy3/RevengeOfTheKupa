using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Encapsulates an action to perform when reaching a game ending state.
/// </summary>
public class GameEndAction : MonoBehaviour
{
	private Animator fader;

	void Start()
	{
        fader = GameObject.FindGameObjectWithTag("ScreenFader").GetComponent<Animator>();
	}

    /// <summary>
    /// The action to perform.
    /// </summary>
    public void onLoss()
	{
		fader.SetTrigger("lose");
	}
}
