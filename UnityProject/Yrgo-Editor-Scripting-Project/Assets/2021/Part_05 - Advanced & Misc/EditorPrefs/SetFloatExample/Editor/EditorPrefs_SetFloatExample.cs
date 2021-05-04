// Simple script that allows a float value to be edited in a slider. 
// The final value is written into the Editor Preferences.
// That value is persistent for the Editor, so you could use that to save settings for your Editor Scripts etc.

using UnityEditor;
using UnityEngine;

public class EditorPrefs_SetFloatExample : EditorWindow
{
	static float floatValue = 0.0f;

	[MenuItem("YRGO/Part 05/EditorPreferences - SetFloat Example")]
	static void Init()
	{
		Rect windowSize = new Rect(10, 10, 300, 150);
		EditorPrefs_SetFloatExample window = EditorWindow.GetWindowWithRect<EditorPrefs_SetFloatExample>(windowSize);
		window.Show();
	}

	void Awake()
	{
		floatValue = EditorPrefs.GetFloat("FloatExample", floatValue);
	}

	void OnGUI()
	{
		floatValue = EditorGUILayout.Slider(floatValue, -1.0f, 1.0f);
		GUILayout.Space(10f);

		if (GUILayout.Button($"Save float to Editor Prefs?"))
			EditorPrefs.SetFloat("FloatExample", floatValue);

		if (GUILayout.Button("Get saved float value!"))
			Debug.Log($"Saved EditorPrefs Float: {EditorPrefs.GetFloat("FloatExample")}");


		GUILayout.Space(20f);
		if (GUILayout.Button("Close"))
			this.Close();
	}
}