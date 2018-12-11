using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that contains on-click methods for the start menu buttons.
/// </summary>
public class EndMenuController : MonoBehaviour
{
	public EventSystem event_system;
	public Sprite win_title_image;
	public Sprite win_background;
	public Sprite loss_title_image;
	public Sprite loss_background;
	public Image title_image;
	public Image bg_image;
	public Text restart_label;
	public GameObject checkpoint_button;
	public GameObject restart_button;

	void Start()
	{
		switch (GameState.state)
		{
			case State.WIN:
				checkpoint_button.SetActive(false);
				bg_image.sprite = win_background;
				title_image.sprite = win_title_image;
				restart_label.text = "Play Again";
				event_system.SetSelectedGameObject(restart_button);
				break;
			case State.LOSS:
				checkpoint_button.SetActive(true);
				bg_image.sprite = loss_background;
				title_image.sprite = loss_title_image;
				restart_label.text = "Restart Game";
				break;
			default:
				break;
		}
	}
}
