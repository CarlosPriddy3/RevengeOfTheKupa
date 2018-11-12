using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for managing life display.
/// </summary>
public class LifeController : MonoBehaviour
{
	public List<GameObject> hearts;
	public GameEndAction action;
	private int num_lives;

	/// <summary>
	/// Initializes the starting number of lives.
	/// </summary>
	void Start ()
	{
		num_lives = hearts.Count;
	}

	/// <summary>
	/// Removes a life from the display. Calls an action if runs out.
	/// </summary>
	public void loseLife()
	{
		if (num_lives > 0)
		{
			num_lives--;
			hearts[num_lives].SetActive(false);
		}
		if (num_lives == 0)
		{
			action.onLoss();
		}
	}

	/// <summary>
	/// Adds a life to the display. Can't add more than initial size.
	/// </summary>
	public void gainLife()
	{
		if (num_lives < hearts.Count)
		{
			hearts[num_lives].SetActive(true);
			num_lives++;
		}
	}
}
