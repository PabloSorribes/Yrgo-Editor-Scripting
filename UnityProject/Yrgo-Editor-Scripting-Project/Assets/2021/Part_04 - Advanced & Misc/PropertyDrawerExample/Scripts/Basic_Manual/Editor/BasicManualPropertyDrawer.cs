using UnityEditor;
using UnityEngine;

/// <summary>
/// This drawer assumes that you specify the '<see cref="amountOfFieldsInProperty"/>' correctly, 
/// ie. you manually check how many fields there are in the Serialized Class that you're doing this Drawer for, 
/// to be able to calculate the total height of the property correctly.
/// </summary>
[CustomPropertyDrawer(typeof(BasicManualSerializedClass))]
public class BasicManualPropertyDrawer : PropertyDrawer
{
	/// <summary>
	/// Update this for how many fields/lines you want to draw in your property.
	/// </summary>
	private readonly int amountOfFieldsInProperty = 2;
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

		//Get references to the properties in the BasicSerializedClass.
		var floatProp = property.FindPropertyRelative(nameof(BasicManualSerializedClass.manualDrawerFloatVariable));
		var boolProp = property.FindPropertyRelative(nameof(BasicManualSerializedClass.manualDrawerBoolVariable));

		//Create custom labels for the two properties
		var floatPropLabel = new GUIContent(text: floatProp.displayName, tooltip: "This float is cool");
		var boolPropLabel = new GUIContent(text: "Custom Bool Name", tooltip: "This bool's name will be this, regardless what the code in BasicSerializedClass says.");

		//Draw the properties, with custom labels
		EditorGUI.PropertyField(position: boolPropRect, property: boolProp, label: boolPropLabel);

		if (boolProp.boolValue)
		{
			EditorGUI.PropertyField(position: floatPropRect, property: floatProp, label: floatPropLabel);
		}


		//Allow prefab logic and Undo/Redo of the values for the property.
		EditorGUI.EndProperty();
		property.serializedObject.ApplyModifiedProperties();
	}

	/// <summary>
	/// Returns the total needed amount of pixels for drawing the property drawer, including all of its fields and labels.
	/// </summary>
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		//This will calculate the total height needed for the property (ie. BasicSerializedClass-label + variables) 
		return (fieldHeight + fieldPaddingY) * amountOfFieldsInProperty;
	}
}