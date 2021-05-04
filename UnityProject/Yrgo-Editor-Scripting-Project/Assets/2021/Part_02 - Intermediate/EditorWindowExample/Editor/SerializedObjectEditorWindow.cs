using UnityEditor;
using UnityEngine;

public class SerializedObjectEditorWindow : EditorWindow
{
	private const string menuItemPath = "YRGO/Part 02/" + myWindowTitle;
	private const string myWindowTitle = "SerializedObjectEditorWindow";

	/// <summary>
	/// Needs to be serialized/public, else we get errors when drawing the UI.
	/// </summary>
	[SerializeField]
	private bool boolField = false;

	[SerializeField]
	private GameObject gameObjectField = null;

	[Range(-10, 10)]
	[SerializeField]
	private int intField = 0;

	[SerializeField]
	private Transform[] arrayField = null;

	private SerializedObject windowSerializedObject = null;
	private SerializedProperty boolProp = null;
	private SerializedProperty gameObjectProp = null;
	private SerializedProperty intProp = null;
	private SerializedProperty arrayProp = null;

	[MenuItem(menuItemPath)]
	public static void SetupWindow()
	{
		//Instantiate the window and set its size.
		var window = GetWindow<SerializedObjectEditorWindow>(utility: false, title: myWindowTitle, focus: true);
		window.minSize = new Vector2(400, 175);
		window.maxSize = new Vector2(window.minSize.x + 10, window.minSize.y + 10);
	}

	// Find all Serialized Properties here so that undo and layout of arrays will be easier to do in OnGUI().
	private void OnEnable()
	{
		windowSerializedObject = new SerializedObject(this);
		boolProp = windowSerializedObject.FindProperty(nameof(boolField));
		gameObjectProp = windowSerializedObject.FindProperty(nameof(gameObjectField));
		intProp = windowSerializedObject.FindProperty(nameof(intField));
		arrayProp = windowSerializedObject.FindProperty(nameof(arrayField));
	}

	public void OnGUI()
	{
		// Always run serializedObject.Update() at the start of a GUI update 
		windowSerializedObject.Update();

		// Draw the bool and array with automatic layout.
		EditorGUILayout.PropertyField(boolProp);
		EditorGUILayout.PropertyField(gameObjectProp);
		EditorGUILayout.PropertyField(intProp);
		EditorGUILayout.PropertyField(arrayProp);

		// This change will show up as "Undo Inspector" and "Redo Inspector" in the "Edit"-menu.
		if (GUILayout.Button("Toggle Bool!"))
		{
			boolProp.boolValue = !boolProp.boolValue;
		}

		// Allows undo on the editor window's properties. 
		// Bear in mind that the GUI may not reflect some changes until the mouse hovers over the window tho.
		windowSerializedObject.ApplyModifiedProperties();
	}
}