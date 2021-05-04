using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlignWithGroundBehaviour_Lesson))]
[CanEditMultipleObjects]
public class AlignWithGroundEditor_Lesson : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		AlignWithGroundBehaviour_Lesson script = (AlignWithGroundBehaviour_Lesson)target;
		if (GUILayout.Button("Ground Object"))
		{
			script.DropChildObjects();
		}

		if (GUILayout.Button("Undo Ground"))
		{
			script.Undo();
		}
	}
}