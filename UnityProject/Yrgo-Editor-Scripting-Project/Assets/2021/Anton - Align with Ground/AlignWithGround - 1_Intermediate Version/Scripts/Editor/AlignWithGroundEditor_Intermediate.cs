using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignWithGroundBehaviour_Intermediate))]
[CanEditMultipleObjects]
public class AlignWithGroundEditor_Intermediate : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		AlignWithGroundBehaviour_Intermediate script = (AlignWithGroundBehaviour_Intermediate)target;
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