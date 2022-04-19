//Originally written by Anton Lindkvist, 2019 09 13
//Additional edits by Pablo Sorribes Bernhard, 2019 09 18

using UnityEditor;
using UnityEngine;

public class SetRandomRotationForObjects_SerializedObjectVersion : EditorWindow
{
	//%&#u translates to "ctrl + alt + shift + r" which has been the keyboard shortcut bound to this window
	//It can otherwise be found under "YRGO/Part 05/SetRandomRotationForObjects_SerializedObjectVersion" in the regular row of menus
	private const string shortcut = " %&#r";
	private const string menuPath = "YRGO/Part 05/" + nameof(SetRandomRotationForObjects_SerializedObjectVersion) + shortcut;

	/// <summary>
	/// Set at construction and used by <see cref="ResetLabelWidth"/> to reset any 
	/// changes made to the <see cref="EditorGUIUtility.labelWidth"/> (by eg. <see cref="CalculateLabelWidth(GUIContent, float)"/>).
	/// </summary>
	private static readonly float originalLabelWidth = EditorGUIUtility.labelWidth;

	//Used to store and handle variables related to the intended script functions
	public bool xRot = true, yRot = true, zRot = true;
	public float rotationOffset = 30f;

	//Allow undo for settings made to the variables in the Editor Window
	private SerializedObject serializedObject;
	private SerializedProperty xRotProp, yRotProp, zRotProp, rotationOffsetProp;

	[MenuItem(menuPath)]
	public static void SetupWindow()
	{
		var window = GetWindow<SetRandomRotationForObjects_SerializedObjectVersion>();
		window.minSize = new Vector2(350, 200);
		window.maxSize = new Vector2(window.minSize.x + 100, window.minSize.y + 100);
	}

	private void OnEnable()
	{
		serializedObject = new SerializedObject(this);
		xRotProp = serializedObject.FindProperty(nameof(this.xRot));
		yRotProp = serializedObject.FindProperty(nameof(this.yRot));
		zRotProp = serializedObject.FindProperty(nameof(this.zRot));
		rotationOffsetProp = serializedObject.FindProperty(nameof(this.rotationOffset));

		//Force the window to Repaint whenever we do an undo/redo for eg. the serializedProperties
		Undo.undoRedoPerformed += Repaint;
	}

	/// <summary>
	/// Allow pressing "Escape" to first clear the Selection, and on next press close the window altogether.
	/// </summary>
	private void HandleInput()
	{
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
		{
			if (Selection.objects.Length >= 1)
			{
				Selection.objects = null;
			}
			else
			{
				//Setting it to null to avoid an error being thrown in serializedObject.ApplyModifiedProperties()
				serializedObject = null;
				this.Close();
			}
		}
	}

	private void OnGUI()
	{
		serializedObject.Update();
		HandleInput();

		//Main box for everything - START
		GUILayout.BeginVertical(GUI.skin.box);

		//A set of bools for selecting which axis I wish to rotate the selection of GameObjects around
		EditorGUILayout.LabelField("Select axises to rotate around");
		GUILayout.BeginHorizontal(GUI.skin.box);
		EditorGUIUtility.labelWidth = CalculateLabelWidth(xRotProp.displayName, 5f);

		//Using SerializedProperty for drawing the checkboxes.
		EditorGUILayout.PropertyField(xRotProp);
		EditorGUILayout.PropertyField(yRotProp);
		EditorGUILayout.PropertyField(zRotProp);

		//Resetting the labelWidth
		ResetLabelWidth();
		GUILayout.EndHorizontal();

		EditorGUILayout.Space();

		//Using a slider to select the max angle I want to have stuff rotated to
		EditorGUILayout.LabelField("Select rotation offset limit");
		GUILayout.BeginVertical(GUI.skin.box);
		EditorGUIUtility.labelWidth = CalculateLabelWidth(rotationOffsetProp.displayName, 5f);
		EditorGUILayout.Slider(property: rotationOffsetProp, leftValue: 0f, rightValue: 180f);
		ResetLabelWidth();
		GUILayout.EndVertical();

		EditorGUILayout.Space();

		//If the rotate button is pressed without having the proper parameters provided, a dialogue box appears
		if (GUILayout.Button($"Apply random rotation to {Selection.gameObjects.Length} objects"))
		{
			if (!xRot && !yRot && !zRot || Selection.gameObjects.Length == 0)
			{
				EditorUtility.DisplayDialog("FATAL ERROR", "Pls check at least one box and select at least 1 object.", " :)))) ");
				return;
			}
			Rotate(Selection.gameObjects, rotationOffset, xRot, yRot, zRot);
		}

		//Resets transforms
		if (GUILayout.Button("Reset selected Transforms"))
		{
			if (Selection.gameObjects.Length != 0)
			{
				ResetRotation(Selection.gameObjects);
			}
		}

		//Main box for everything - END
		GUILayout.EndVertical();

		//Doing a null-check to avoid an error being thrown when using ESC-key to close the window.
		serializedObject?.ApplyModifiedProperties();
	}

	/// <summary>
	/// Applies the actual rotation for each object in the selection array
	/// </summary>
	private void Rotate(GameObject[] gameObjects, float offsetMax, bool xRot = false, bool yRot = false, bool zRot = false)
	{
		foreach (GameObject gameObject in gameObjects)
		{
			var vector = gameObject.transform.localEulerAngles;
			if (xRot)
				vector.x = Random.Range(-offsetMax, offsetMax);
			if (yRot)
				vector.y = Random.Range(-offsetMax, offsetMax);
			if (zRot)
				vector.z = Random.Range(-offsetMax, offsetMax);

			Undo.RecordObject(gameObject.transform, "Rotation applied");
			gameObject.transform.localEulerAngles = vector;
		}
	}

	/// <summary>
	/// Reset the rotation of all selected transforms to Vector3.zero.
	/// </summary>
	private void ResetRotation(GameObject[] gameObjects)
	{
		Undo.RecordObjects(gameObjects, "Reset Rotation");

		foreach (GameObject obj in gameObjects)
		{
			obj.transform.localEulerAngles = Vector3.zero;
		}
	}

	/// <summary>
	/// Useful for setting the <see cref="EditorGUIUtility.labelWidth"/> to the width of your desired label.
	/// </summary>
	/// <param name="label"></param>
	/// <returns></returns>
	public static float CalculateLabelWidth(GUIContent label, float padding = 0.0f)
	{
		float labelwidth = GUI.skin.label.CalcSize(label).x + padding;
		return labelwidth;
	}

	/// <summary>
	/// Useful for setting the <see cref="EditorGUIUtility.labelWidth"/> to the width of your desired label 
	/// by just inputting a string instead of a complete GUIContent.
	/// </summary>
	/// <param name="label"></param>
	/// <returns></returns>
	public static float CalculateLabelWidth(string label, float padding = 0.0f)
	{
		return CalculateLabelWidth(new GUIContent(label), padding);
	}

	public static void ResetLabelWidth()
	{
		EditorGUIUtility.labelWidth = originalLabelWidth;
	}
}