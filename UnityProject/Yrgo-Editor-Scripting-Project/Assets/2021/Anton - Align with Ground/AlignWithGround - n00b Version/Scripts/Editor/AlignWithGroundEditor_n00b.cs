using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignWithGroundBehaviour_n00b))]
[CanEditMultipleObjects]
public class AlignWithGroundEditor_n00b : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		AlignWithGroundBehaviour_n00b script = (AlignWithGroundBehaviour_n00b)target;
		if (GUILayout.Button("Ground Object"))
		{
			script.DropChildObjects();
		}

		if (GUILayout.Button("Undo Ground"))
		{
			script.Undo();
		}

		if (GUILayout.Button("Redo Ground"))
		{
			script.Redo();
		}
	}
}