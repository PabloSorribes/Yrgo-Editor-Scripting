using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Lesson_BasicBehaviour))]
public class Lesson_BasicCustomInspector : Editor
{
	public override void OnInspectorGUI()
	{
		//Updates the object's state/view/representation based on
		//the changes from the last frame.
		serializedObject.Update();

		//Optional - draw the default inspector
		base.OnInspectorGUI();

		//Do your editor stuff here:
		// TODO: Find string property & display it via "EditorGUILayout.PropertyField()"


		// TODO: Show/hide value depending on bool

		//Apply the occurred changes with this line (allow automatic undo/redo)
		serializedObject.ApplyModifiedProperties();
	}
}
