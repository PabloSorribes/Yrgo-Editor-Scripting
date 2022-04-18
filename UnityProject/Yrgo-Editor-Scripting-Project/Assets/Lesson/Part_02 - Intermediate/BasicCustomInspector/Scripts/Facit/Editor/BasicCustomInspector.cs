using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(BasicBehaviour))]
public class BasicCustomInspector : Editor
{
	public override void OnInspectorGUI()
	{
		//Updates the object's state/view/representation based on
		//the changes from the last frame.
		serializedObject.Update();

		//Optional - draw the default inspector
		base.OnInspectorGUI();

		//Do your editor stuff here:

		//Get a variable by name, make a field for it and print its value
		var stringProperty = serializedObject.FindProperty(nameof(BasicBehaviour.myVariableField));
		EditorGUILayout.PropertyField(stringProperty);

		// Easy way of checking a public bool on the BasicBehaviour
		if (target is BasicBehaviour basicBehaviour)
		{
			if (basicBehaviour.debugValueInOnGUI)
				Debug.Log(stringProperty.stringValue);
		}

		// Draw buttons which do stuff
		if (GUILayout.Button("Button 01 - Do something"))
		{
			stringProperty.stringValue = "Do something";
			Debug.Log($"Pressed Button 01 - {stringProperty.name} = '{stringProperty.stringValue}'");
		}

		if (GUILayout.Button("Button 02 - Do another thing"))
		{
			stringProperty.stringValue = "Do another thing";
			Debug.Log($"Pressed Button 02 - {stringProperty.name} = '{stringProperty.stringValue}'");
		}

		//Apply the occurred changes with this line (allow automatic undo/redo)
		serializedObject.ApplyModifiedProperties();
	}
}
