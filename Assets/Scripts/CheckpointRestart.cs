using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to encapsulate checkpoint restart action.
/// </summary>
public class CheckpointRestart : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{

	}

	/// <summary>
	/// Restarts from the checkpoint.
	/// </summary>
	public void restart()
	{
		Debug.Log("Restarted from checkpoint");
	}
}
