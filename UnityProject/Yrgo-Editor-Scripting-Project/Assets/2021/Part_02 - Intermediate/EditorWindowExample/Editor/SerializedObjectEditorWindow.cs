using UnityEditor;
using UnityEngine;

public class SerializedObjectEditorWindow : EditorWindow
{
	private const string menuItemPath = "Examples/" + myWindowTitle;
	private const string myWindowTitle = "SerializedObjectEditorWindow";

	/// <summary>
	/// Needs to be serialized/public, else we get errors when drawing the UI.
	/// </summary>
	[SerializeField]
	private Transform[] arrayField = null;

	private SerializedObject windowSerializedObject = null;
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
		arrayProp = windowSerializedObject.FindProperty(nameof(arrayField));
	}

	public void OnGUI()
	{
		// Always run serializedObject.Update() at the start of a GUI update 
		windowSerializedObject.Update();

		// Draw the array, no biggie
		EditorGUILayout.PropertyField(arrayProp);

		// Allows undo on the editor window's properties. 
		// Bear in mind that the GUI may not reflect some changes until the mouse hovers over the window tho.
		windowSerializedObject.ApplyModifiedProperties();
	}
}