using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains on-click methods for the start menu buttons.
/// </summary>
public class EndMenuController : MonoBehaviour
{
	public Sprite win_title_image;
	public Sprite win_background;
	public Sprite loss_title_image;
	public Sprite loss_background;
	public Image title_image;
	public Image bg_image;
	public Text restart_label;

	void Start()
	{
		switch (GameState.state)
		{
			case State.WIN:
				bg_image.sprite = win_title_image;
				title_image.sprite = win_background;
				restart_label.text = "Play Again";
				break;
			case State.LOSS:
				bg_image.sprite = loss_background;
				title_image.sprite = loss_title_image;
				restart_label.text = "Try Again";
				break;
			default:
				break;
		}
	}
}
