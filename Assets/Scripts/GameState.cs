using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game state from a scene standpoint.
/// </summary>
public enum State
{
	START,
	PLAY,
	WIN,
	LOSS
}

/// <summary>
/// Contains static variable with game's current state.
/// </summary>
public class GameState : MonoBehaviour
{
	public static State state = State.START;
	public static bool paused = false;
}
