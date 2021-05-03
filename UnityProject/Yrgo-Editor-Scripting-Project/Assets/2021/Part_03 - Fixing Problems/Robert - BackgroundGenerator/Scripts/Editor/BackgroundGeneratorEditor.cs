using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BackgroundGenerator))]
public class BackgroundGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		BackgroundGenerator myScript = (BackgroundGenerator)target;

		//TODO: Add undo functionality

		if (GUILayout.Button("Generate"))
		{
			myScript.Generate();
		}

		if (GUILayout.Button("Clear"))
		{
			myScript.Clear();
		}
	}
}
