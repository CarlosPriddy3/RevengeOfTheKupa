using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Encapsulates an action to perform when reaching a game ending state.
/// </summary>
public class GameEndAction : MonoBehaviour
{
	public string end_scene_name;

    private SoundManager soundManager;

    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    /// <summary>
    /// The action to perform.
    /// </summary>
    public void onLoss()
	{
        soundManager.PlayDefeatMusic();
		SceneManager.LoadSceneAsync(end_scene_name);
		GameState.state = State.LOSS;
	}
}
