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

		// TODO: Show/hide value depending on bool
		SerializedProperty boolProp = serializedObject.FindProperty(nameof(Lesson_BasicBehaviour.showField));

		if (boolProp.boolValue)
		{
			//Do your editor stuff here:
			// TODO: Find string property & display it via "EditorGUILayout.PropertyField()"
			SerializedProperty stringProp = serializedObject.FindProperty(nameof(Lesson_BasicBehaviour.myVariableField));
			EditorGUILayout.PropertyField(stringProp);
		}

		if (GUILayout.Button("Toggle Bool Value"))
		{
			boolProp.boolValue = !boolProp.boolValue;
		}


		//Apply the occurred changes with this line (allow automatic undo/redo)
		serializedObject.ApplyModifiedProperties();
	}
}
