using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor.Animations;

/// <summary>
/// Controls the intro cutscene.
/// </summary>
public class CutsceneController : MonoBehaviour
{
	public Animator anim;
	public Image image;
	public Text text;
	public Text skip_text;
	public List<Sprite> images;
	public List<string> texts;

	public string game_scene;
	public string tutorial_scene;

	private SoundManager soundManager;
	private int current;
	private bool ended;

	// Use this for initialization
	void Start ()
	{
		soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
		current = 0;
	}

	// Update is called once per frame
	void Update ()
	{
		AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
		if (state.IsName("Blank State"))
		{
			if (current >= images.Count && !ended)
			{
				Color transparent = Color.white;
				transparent.a = 0;
				skip_text.color = transparent;
				startGame();
			}
			else
			{
				setDisplayItems();
				anim.SetBool("fading", true);
			}
		}
		else if (Input.anyKeyDown)
		{
			if (!Input.GetButton("Menu"))
			{
				if (state.IsName("Visible State"))
				{
					current++;
					anim.SetBool("fading", true);
				}
				else
				{
					if (state.IsName("Fade In"))
					{
						anim.Play("Fade In", 0, 0.99f);
					}
					else if (state.IsName("Fade Out"))
					{
						anim.Play("Fade Out", 0, 0.99f);
					}
				}
			}
			else
			{
				current = images.Count;
				anim.Play("Fade Out", 0, 0.99f);
			}
		}
	}

	/// <summary>
	/// Sets the image and text to be displayed.
	/// </summary>
	private void setDisplayItems()
	{
		image.sprite = images[current];
		text.text = texts[current];
	}

	/// <summary>
	/// Starts the game or tutorial.
	/// </summary>
	private void startGame()
	{
		GameState.state = State.PLAY;
		soundManager.PlayGameMusic();
		if (SelectedStart.isTutorial)
		{
			SceneManager.LoadScene(tutorial_scene);
		}
		else
		{
			SceneManager.LoadScene(game_scene);
		}
		ended = true;
	}
}
