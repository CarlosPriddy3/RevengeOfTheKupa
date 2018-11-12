using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the game timer.
/// </summary>
public class TimerController : MonoBehaviour {

	public Text countdown_text;
	public int max_time;
	public GameEndAction action;

	private int curr_time;

	/// <summary>
	/// Initialization
	/// </summary>
	void Start ()
	{
		curr_time = max_time;
		StartCoroutine(StartCountdown());
	}

	/// <summary>
	/// Coroutine that sets the timer UI to the current time remaining
	/// every second. Once current time remaining reaches 0, performs
	/// the specified action.
	/// </summary>
	public IEnumerator StartCountdown()
	{
		while (curr_time > 0)
		{
			setTimerText();
			yield return new WaitForSeconds(1.0f);
			curr_time--;
		}
		action.onLoss();
	}

	/// <summary>
	/// Sets the timer UI text with the current time remaining. If ten seconds
	/// or less remain, text is red.
	/// </summary>
	private void setTimerText()
	{
		countdown_text.text = curr_time.ToString();
		if (curr_time <= 10)
		{
			countdown_text.color = Color.red;
		}
	}
}
