using UnityEditor;
using UnityEngine;

/// <summary>
/// "Automatic" in that we calculate the amount of visible/serialized fields in the class, and change the property's height depending on that.
/// We still need to write the GUI-code for each property tho.
/// </summary>
[CustomPropertyDrawer(typeof(AutomaticBasicSerializedClass))]
public class AutomaticPropertyDrawer : PropertyDrawer
{
	private readonly float fieldHeight = EditorGUIUtility.singleLineHeight;
	private readonly float fieldPaddingY = 2f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		property.serializedObject.Update();
		EditorGUI.BeginProperty(position, label, property);

		//Showing that you can change the properties of your label.
		var prefixLabelContent = label;
		prefixLabelContent.text = "The Basic Serialized Class";
		prefixLabelContent.tooltip = "Showcasing a custom property drawer.";

		//Draw label AND position it and the rest of it's children correctly (because of PrefixLabel)
		//
		//"PrefixLabel" adds a field for the name of the variable being drawn (in this case the variable for the BasicSerializedClass).
		//Additionally, all fields end up being indented a bit to the right of the label.
		position = EditorGUI.PrefixLabel(totalPosition: position, id: GUIUtility.GetControlID(FocusType.Passive), label: prefixLabelContent, style: EditorStyles.largeLabel);

		//offset position.y by field size & Calculate Rects.
		//Save the startPos to add it if you add more fields.
		float startPosY = position.y;
		var floatPropRect = new Rect(position.x, startPosY, position.width, fieldHeight);   //First prop rect
		startPosY += fieldHeight + fieldPaddingY;                                           //Offset startPosY by field size for each new line of properties
		var boolPropRect = new Rect(position.x, startPosY, position.width, fieldHeight);    //Next prop rect
		startPosY += fieldHeight + fieldPaddingY;
		var extraBoolPropRect = new Rect(position.x, startPosY, position.width, fieldHeight);

		//Get references to the properties in the BasicSerializedClass.
		var floatProp = property.FindPropertyRelative(nameof(AutomaticBasicSerializedClass.drawerFloatVariable));
		var boolProp = property.FindPropertyRelative(nameof(AutomaticBasicSerializedClass.drawerBoolVariable));
		var extraBoolProp = property.FindPropertyRelative(nameof(AutomaticBasicSerializedClass.drawerExtraBool));

		//Create custom labels for the properties
		var floatPropLabel = new GUIContent(text: floatProp.displayName, tooltip: "This float is cool");
		var boolPropLabel = new GUIContent(text: "Custom Bool Name", tooltip: "This bool's name will be this, regardless what the code in BasicSerializedClass says.");
		var extraBoolPropLabel = new GUIContent(text: extraBoolProp.displayName, tooltip: "An extra bool");

		//Draw the properties, with custom labels
		EditorGUI.PropertyField(position: floatPropRect, property: floatProp, label: floatPropLabel);
		EditorGUI.PropertyField(position: boolPropRect, property: boolProp, label: boolPropLabel);
		EditorGUI.PropertyField(position: extraBoolPropRect, property: extraBoolProp, label: extraBoolPropLabel);

		//Allow prefab logic and Undo/Redo of the values for the property.
		EditorGUI.EndProperty();
		property.serializedObject.ApplyModifiedProperties();
	}

	/// <summary>
	/// Counts all visible fields in the monobehaviour-script, starting at the main property (ie. <see cref="AutomaticBasicSerializedClass"/>)
	/// and all the way down (cause Unity is stupid), and with that info it calculates how many public/serialized fields there are in
	/// the main property (<see cref="AutomaticBasicSerializedClass"/>).
	/// </summary>
	/// <param name="property"></param>
	/// <returns>The visible/serialized fields (if any) in the <paramref name="property"/>.</returns>
	private static int CalculateAmountOfFieldsInMainProperty(ref SerializedProperty property)
	{
		//Save the original path to reset the property to its default values later on.
		string originalPath = property.propertyPath;

		//Will loop through all the visible fields on the main script EXCEPT for the ones
		//in the actual serialized class that we're doing a drawer for.
		int notDrawerVars = 0;
		while (property.NextVisible(false))
		{
			//Debug.Log(property.name);
			notDrawerVars++;
		}

		//Reset to the start of the serializedObject and set the property back to its original position/value.
		property = property.serializedObject.FindProperty(originalPath);

		//Loop through ALL the visible fields on the main script,
		//including the variables/properties in the serialized class 
		//we're doing this drawer for.
		int allVars = 0;
		while (property.NextVisible(true))
		{
			//Debug.Log(property.name);
			allVars++;
		}

		//Reset the property again after the last loop and shit
		property = property.serializedObject.FindProperty(originalPath);

		int myDrawerVars = allVars - notDrawerVars;
		//Debug.Log("MyVars: " + myDrawerVars);

		return myDrawerVars;
	}

	/// <summary>
	/// Returns the total needed amount of pixels for drawing the property drawer, including all of its fields and labels.
	/// </summary>
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		//This will calculate the total height needed for the property (ie. BasicSerializedClass-label + variables) 
		//as well as add some padding for each line to look good.
		return (fieldHeight + fieldPaddingY) * CalculateAmountOfFieldsInMainProperty(ref property);
	}
}
