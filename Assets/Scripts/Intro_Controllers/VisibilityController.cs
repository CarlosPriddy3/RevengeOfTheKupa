using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the visibility of objects in the intro cutscene.
/// </summary>
public class VisibilityController : MonoBehaviour
{
	public Image image;
	public Text text;
	public Image icon;

	/// <summary>
	/// Makes the icon visible.
	/// </summary>
	public void showIcon()
	{
		setIconVisiblity(true);
	}

	/// <summary>
	/// Hides the icon.
	/// </summary>
	public void hideIcon()
	{
		setIconVisiblity(false);
	}

	/// <summary>
	/// Makes every element visible.
	/// </summary>
	public void showAll()
	{
		setImageVisiblity(true);
		setTextVisiblity(true);
		setIconVisiblity(true);
	}

	/// <summary>
	/// Hides every element.
	/// </summary>
	public void hideAll()
	{
		setImageVisiblity(true);
		setTextVisiblity(true);
		setIconVisiblity(true);
	}

	/// <summary>
	/// Sets the visibilty of the cutscene image.
	/// </summary>
	/// <param name="visible">Whether or not to set it as visible.</param>
	private void setImageVisiblity(bool visible)
	{
		Color color = Color.white;
		if (visible)
		{
			color.a = 1;
		}
		else
		{
			color.a = 0;
		}
		image.color = color;
	}

	/// <summary>
	/// Sets the visibilty of the cutscene text.
	/// </summary>
	/// <param name="visible">Whether or not to set it as visible.</param>
	private void setTextVisiblity(bool visible)
	{
		Color color = Color.white;
		if (visible)
		{
			color.a = 1;
		}
		else
		{
			color.a = 0;
		}
		text.color = color;
	}

	/// <summary>
	/// Sets the visibilty of the icon.
	/// </summary>
	/// <param name="visible">Whether or not to set it as visible.</param>
	private void setIconVisiblity(bool visible)
	{
		Color color = Color.white;
		if (visible)
		{
			color.a = 1;
		}
		else
		{
			color.a = 0;
		}
		icon.color = color;
	}
}
