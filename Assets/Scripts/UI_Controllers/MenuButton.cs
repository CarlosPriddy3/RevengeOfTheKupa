using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Button extension with specialized OnSelect/OnDeselect actions.
/// </summary>
public class MenuButton : Button
{
	// Label for the button.
	private Text label;

	// Colors used for displaying selected/non-selected buttons.
	public Color def_text_color = new Color(0.961f, 0.961f, 0.961f, 1.0f);
	public Color select_text_color = new Color(0.059f, 0.059f, 0.059f, 1.0f);
	private Color no_color = new Color(1f, 1f, 1f, 0f);

	protected override void Start ()
	{
		base.Start();
		// Get components.
		label = GetComponentInChildren<Text>();
		if (label == null)
		{
			Debug.LogError("Label not found");
		}
		setDefaultColors();
	}

	public override void OnSelect(BaseEventData eventData)
	{
		setSelectedColors();
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		setDefaultColors();
	}

	/// <summary>
	/// Sets the button and label's colors to the "selected" color set.
	/// </summary>
	private void setSelectedColors()
	{
		ColorBlock block = new ColorBlock();
		block.disabledColor = no_color;
		block.highlightedColor = def_text_color;
		block.normalColor = def_text_color;
		block.pressedColor = def_text_color;
		block.colorMultiplier = 1f;
		block.fadeDuration = 0.1f;
		colors = block;
		label.color = select_text_color;
	}

	/// <summary>
	/// Sets the button and label's colors to the "non-selected" color set.
	/// </summary>
	private void setDefaultColors()
	{
		ColorBlock block = new ColorBlock();
		block.disabledColor = no_color;
		block.highlightedColor = no_color;
		block.normalColor = no_color;
		block.pressedColor = def_text_color;
		block.colorMultiplier = 1f;
		block.fadeDuration = 0.1f;
		colors = block;
		label.color = def_text_color;
	}
}
