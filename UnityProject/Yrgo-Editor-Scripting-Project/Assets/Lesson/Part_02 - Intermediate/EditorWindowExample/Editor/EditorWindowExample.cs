using UnityEditor;
using UnityEngine;

public class EditorWindowExample : EditorWindow
{
	private const string menuItemPath = "YRGO/Part 02/" + myWindowTitle;
	private const string myWindowTitle = "My Editor Window Example";

	public string myString = "My awesome string";
	public GameObject myGameObjectField = null;
	public int myIntSlider = 0;
	public bool myBool = false;

	[MenuItem(menuItemPath)]
	public static void SetupWindow()
	{
		//Instantiate the window and set its size.
		var window = GetWindow<EditorWindowExample>(utility: false, title: myWindowTitle, focus: true);
		window.minSize = new Vector2(400, 175);
		window.maxSize = new Vector2(window.minSize.x + 10, window.minSize.y + 10);
	}


	private float timer = 0;
	private float lastTime = 0;
	private float DeltaTime { get { return Time.realtimeSinceStartup - lastTime; } }

	private void OnEnable()
	{
		//This update is called 30 times per second in the Editor.
		//Basically an Update() you could use at Edit Time, if you like or need it.
		EditorApplication.update += MyOnUpdate;
	}

	private void OnDisable()
	{
		EditorApplication.update -= MyOnUpdate;
	}

	private void MyOnUpdate()
	{
		CalculateDeltaTime();
	}

	private void CalculateDeltaTime()
	{
		timer += DeltaTime;
		lastTime = Time.realtimeSinceStartup;
		//Debug.Log($"Timer: {timer}");
	}

	// Needs to be called from OnGUI, else it won't work.
	private void HandleKeyboardInput()
	{
		//If we press escape while the Window is focused, we Close it.
		if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
		{
			Debug.Log("ESC!");
			this.Close();
		}
	}

	private void OnGUI()
	{
		HandleKeyboardInput();

		//Start the main vertical box, which will give the tool a coherent background.
		EditorGUILayout.BeginVertical(GUI.skin.box);

		GUI_DrawStringField();
		GUI_DrawObjectField();
		GUI_DrawIntSlider();

		//Add some space between the rest of the controls and the bool.
		EditorGUILayout.Space();
		GUI_DrawBoolAndFirstButton();

		//Add some space between the bool-button group and the last button.
		EditorGUILayout.Space();
		GUI_DrawSecondButton();

		//Finally, end the main vertical box, which gave the coherent background.
		EditorGUILayout.EndVertical();
	}

	private void GUI_DrawStringField()
	{
		//--- STRING ---//
		//First we create the label-text with a tooltip.
		//Then we create a text field, supply it with the label we just created and 
		//insert the value it is supposed to write out and apply it to the string-variable.
		EditorGUILayout.BeginHorizontal(GUI.skin.box);
		GUIContent myStringLabel = new GUIContent(text: "My String: ", tooltip: "What's your string all about?");
		myString = EditorGUILayout.TextField(myStringLabel, myString);
		EditorGUILayout.EndHorizontal();
	}

	private void GUI_DrawObjectField()
	{
		//--- OBJECT FIELD ---//
		//We also use "CalculateLabelWidth()" to adapt the right hand side of the field to the length of our label's text-field (normally the variable name).
		//Try commenting out that line and see what happens.
		EditorGUILayout.BeginHorizontal(GUI.skin.box);
		GUIContent myGameObjectFieldLabel = new GUIContent(text: "A really long description for an object field: ", tooltip: "Drag n drop a gameObject here!");
		EditorGUIUtility.labelWidth = CalculateLabelWidth(myGameObjectFieldLabel);
		myGameObjectField = (GameObject)EditorGUILayout.ObjectField(myGameObjectFieldLabel, myGameObjectField, typeof(GameObject), allowSceneObjects: true);
		ResetLabelWidth();
		EditorGUILayout.EndHorizontal();
	}

	private void GUI_DrawIntSlider()
	{
		//--- INT SLIDER ---//
		//Here we use "nameof()" to avoid having to update the variable-text name in case we rename the actual variable.
		//See how the layout changes with or without the "CalculateLabelWidth(myIntLabel)"-line and the "ResetLabelWidth()"-line.
		EditorGUILayout.BeginHorizontal(GUI.skin.box);
		GUIContent myIntSliderLabel = new GUIContent(text: nameof(myIntSlider), "Yeah, drag my values bae.");
		//EditorGUIUtility.labelWidth = CalculateLabelWidth(myIntSliderLabel);
		myIntSlider = EditorGUILayout.IntSlider(label: myIntSliderLabel, value: myIntSlider, leftValue: -10, rightValue: 10);
		//ResetLabelWidth();
		EditorGUILayout.EndHorizontal();
	}

	private void GUI_DrawBoolAndFirstButton()
	{
		//--- BOOL & FIRST BUTTON ---//
		//Here we begin another Vertical group inside the new Horizontal group 
		//to section off the bool and the first button from the rest.
		//We also change the background color of everything to make it pop out more.
		//Take care to cache the original color for reapplying it later.
		var originalBackgroundColor = GUI.backgroundColor;
		GUI.backgroundColor = Color.cyan;
		EditorGUILayout.BeginHorizontal(GUI.skin.box);
		EditorGUILayout.BeginVertical();

		//-- MAKE A BOOL LABEL --//
		//Here we show that you can input the parameters without naming them (normal way, but way less clear what each one does).
		GUIContent myBoolLabel = new GUIContent("Enable GameObjects on Button-press: ", "Yeah, flip my values bae.");
		EditorGUIUtility.labelWidth = CalculateLabelWidth(myBoolLabel);
		myBool = EditorGUILayout.Toggle(myBoolLabel, myBool);
		ResetLabelWidth();

		//-- MAKE FIRST BUTTON --//
		//Display a button to perform an action with.
		//Change its text depending on the amount of selected objects and the value of the previous bool.
		//
		//Free C#-lesson:
		//Using string-interpolation ($"MyString {myVariable}.") is better for performance than adding several strings together. 
		//It also makes your code easier to read when adding multiple strings and variables together.
		//Read more here: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
		GUI.backgroundColor = Color.red;
		var selectedObjects = Selection.gameObjects;
		if (GUILayout.Button($"Set {selectedObjects.Length} selected objects' enabled status to: {myBool}."))
		{
			if (selectedObjects.Length < 1)
			{
				//Make a Popup dialogue which you can use to display important info to your user.
				EditorUtility.DisplayDialog(title: "No objects selected!",
					message: "You have to select some objects to be able to change their state!",
					ok: "Ok, I'll select some objects :)");
				return;
			}

			//Record the entire array of objects, since we're gonna loop over all of them either way.
			Undo.RecordObjects(selectedObjects, $"Setting active state to {myBool}");
			foreach (var obj in selectedObjects)
			{
				obj.SetActive(myBool);
			}
		}

		//End the sub-group and reset the background color.
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUI.backgroundColor = originalBackgroundColor;
	}

	private void GUI_DrawSecondButton()
	{
		//--- SECOND BUTTON ---//
		//How to make a Popup-dialogue with two options.
		//"EditorUtility.DisplayDialog" returns a bool depending on which button was clicked. 
		//Use this to perform different actions depending on the user's input.
		if (GUILayout.Button($"Popup dialogue example!"))
		{
			if (EditorUtility.DisplayDialog(title: "Warning: The popup dialog was popped!",
					message: "This is how you tell artists and all other non-coders how to not mess with your tool.",
					ok: "Ok, I'll follow the Dev's instructions :)",
					cancel: "Nah, cancel this!"))
			{
				Debug.Log("Popup Window: Ok-button was clicked.");
			}
			else
			{
				//This statement also triggers if the user closes the popup using the X at the top right of the window.
				Debug.Log("Popup Window: Cancel-button was clicked.");
			}
		}
	}


	/// <summary>
	/// Useful for setting the <see cref="EditorGUIUtility.labelWidth"/> to the width of your desired label.
	/// </summary>
	/// <param name="label"></param>
	/// <returns></returns>
	public static float CalculateLabelWidth(GUIContent label, float padding = 0f)
	{
		float labelWidth = GUI.skin.label.CalcSize(label).x + padding;
		return labelWidth;
	}

	/// <summary>
	/// Set at construction and used by <see cref="ResetLabelWidth"/> to reset any 
	/// changes made to the <see cref="EditorGUIUtility.labelWidth"/> (by eg. <see cref="CalculateLabelWidth(GUIContent, float)"/>).
	/// </summary>
	private static readonly float originalLabelWidth = EditorGUIUtility.labelWidth;

	/// <summary>
	/// Reset the <see cref="EditorGUIUtility.labelWidth"/> to the default value.
	/// </summary>
	public static void ResetLabelWidth()
	{
		EditorGUIUtility.labelWidth = originalLabelWidth;
	}
}
