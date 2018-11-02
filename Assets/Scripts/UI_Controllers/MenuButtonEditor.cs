#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(MenuButton))]
public class MenuButtonEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MenuButton button = (MenuButton) target;
		button.icon = (Image) EditorGUILayout.ObjectField("Icon", button.icon, typeof(Image), true);
		// Show default inspector property editor
		DrawDefaultInspector();
	}
}
#endif