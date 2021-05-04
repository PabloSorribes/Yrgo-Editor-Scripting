// Written by Pablo Sorribes Bernhard, 2019 02 16

using UnityEditor;
using UnityEngine;

/// <summary>
/// Note that "property" in this case means the entire drawer for all the fields and labels.
/// </summary>

// Tutorial showing how to draw a PropertyDrawer: https://riptutorial.com/unity3d/example/8282/custom-property-drawer
// Drawing an actual property field: https://youtu.be/-bxaYugwVL4?t=1507
[CustomPropertyDrawer(typeof(MinMaxSlider))]
public class MinMaxSliderDrawer : PropertyDrawer
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
		EditorGUI.BeginProperty(position, label, property);

		var prefixLabelContent = label;
		prefixLabelContent.tooltip = $"Slider allowing you to set your custom ranges, and get the respective min/max values set in the slider.";

		// Draw label.
		// "PrefixLabel" adds a field for the name of the variable being drawn (in this case the variable for the MinMaxSlider-class).
		// All Fields end up being indented a bit to the right of the label.
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), prefixLabelContent);

		// Create labels for each element (are used later)
		// "Min/Max Range" Float-fields.
		var minSliderLabel = new GUIContent("Min Range", "Lower total range for the slider.");
		var maxSliderLabel = new GUIContent("Max Range", "Upper total range for the slider.");

		// "Min/Max Value" Float-fields.
		var minLabel = new GUIContent("Min", "Minimum Value returned from the slider.");
		var maxLabel = new GUIContent("Max", "Maximum Value returned from the slider.");

		int oldIndent;
		SetIndent(out oldIndent, 0);


		#region Field Positioning
		// Getting fieldwiths which will be relative to how much text is in the label (ie. no more pixel-pushing guesswork), 
		// as well as how many float characters should be allowed in the float field.
		var padding = 5;
		var fieldWidth_RangeFloats = GUI.skin.label.CalcSize(minSliderLabel).x + GUI.skin.label.CalcSize(new GUIContent("1.23")).x + padding;
		var fieldWidth_ValueFloats = GUI.skin.label.CalcSize(minLabel).x + GUI.skin.label.CalcSize(new GUIContent("1.23")).x + padding;
		var sliderHorizontalOffset = fieldWidth_ValueFloats + 10f;

		// offset position.y by field size & Calculate Rects.
		// Save the start pos to add it if you add more fields.
		float startPosY = position.y;
		var minSliderRangeLabelRect = new Rect(position.x, startPosY, fieldWidth_RangeFloats, fieldHeight);
		var maxSliderRangeLabelRect = new Rect(position.x + position.width - fieldWidth_RangeFloats, startPosY, fieldWidth_RangeFloats, fieldHeight);

		// offset position.y by field size
		startPosY += fieldHeight + fieldPaddingY;
		var minValLabelRect = new Rect(position.x, startPosY, fieldWidth_ValueFloats, fieldHeight);
		var maxValLabelRect = new Rect(position.x + position.width - fieldWidth_ValueFloats, startPosY, fieldWidth_ValueFloats, fieldHeight);
		var sliderRect = new Rect(position.x + sliderHorizontalOffset, startPosY, position.width - (sliderHorizontalOffset * 2), fieldHeight);
		#endregion Field Positioning


		#region Get Properties & Create float values
		// Get references to the properties for a slider's minimum/maximum range.
		var minSliderRangeProp = property.FindPropertyRelative(nameof(MinMaxSlider.sliderRangeMin));
		var maxSliderRangeProp = property.FindPropertyRelative(nameof(MinMaxSlider.sliderRangeMax));
		float minSliderRangeVal = minSliderRangeProp.floatValue;
		float maxSliderRangeVal = maxSliderRangeProp.floatValue;

		// Get references to the min/max value-properties and their float values.
		var minProp = property.FindPropertyRelative(nameof(MinMaxSlider.minVal));
		var maxProp = property.FindPropertyRelative(nameof(MinMaxSlider.maxVal));
		float minval = minProp.floatValue;
		float maxval = maxProp.floatValue;
		#endregion Get Properties & Create float values


		#region Draw Min/Max Range of Sliders
		// Draw MinMax Slider float fields without shittons of label padding
		var originalLabelWidth = EditorGUIUtility.labelWidth;

		// Values relative to the size of the text in the minSliderLabel (make the float field begin after the label text).
		float sliderFloatsLabelWidth = GUI.skin.label.CalcSize(minSliderLabel).x + padding;
		EditorGUIUtility.labelWidth = sliderFloatsLabelWidth;
		minSliderRangeVal = EditorGUI.FloatField(minSliderRangeLabelRect, minSliderLabel, minSliderRangeVal);
		maxSliderRangeVal = EditorGUI.FloatField(maxSliderRangeLabelRect, maxSliderLabel, maxSliderRangeVal);
		EditorGUIUtility.labelWidth = originalLabelWidth;

		// Clamp slider's min/max range to stop from crossing each other and balla ur.
		minSliderRangeVal = Mathf.Clamp(minSliderRangeVal, float.MinValue, maxSliderRangeVal);
		maxSliderRangeVal = Mathf.Clamp(maxSliderRangeVal, minSliderRangeVal, float.MaxValue);

		// Assign clamped values to slider's min/max range.
		minSliderRangeProp.floatValue = minSliderRangeVal;
		maxSliderRangeProp.floatValue = maxSliderRangeVal;

		// Draw MinMaxSlider and save previous values to clamp the result later on.
		EditorGUI.MinMaxSlider(sliderRect, ref minval, ref maxval, minSliderRangeVal, maxSliderRangeVal);
		float previousMaxVal = maxval;
		#endregion Draw Min/Max Range of Sliders


		#region Draw Min/Max Value Floats
		// Draw Float fields without shittons of label padding
		originalLabelWidth = EditorGUIUtility.labelWidth;

		// Values relative to the size of the text in the minLabel (make the float field begin after the label text).
		EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(minLabel).x + padding;
		minval = EditorGUI.FloatField(minValLabelRect, minLabel, minval);
		maxval = EditorGUI.FloatField(maxValLabelRect, maxLabel, maxval);
		EditorGUIUtility.labelWidth = originalLabelWidth;

		// Clamp the values to stop them from crossing each other.
		minval = Mathf.Clamp(minval, minSliderRangeVal, previousMaxVal);
		maxval = Mathf.Clamp(maxval, minval, maxSliderRangeVal);

		// Set the clamped slider/float field values.
		minProp.floatValue = minval;
		maxProp.floatValue = maxval;
		#endregion Draw Min/Max Value Floats

		// Reset indent and update the variable.
		SetIndent(out oldIndent, oldIndent);

		// Allow prefab logic and Undo/Redo on the values of the Sliders.
		EditorGUI.EndProperty();
		property.serializedObject.ApplyModifiedProperties();
	}

	/// <summary>
	/// Returns the total needed amount of pixels for drawing the property drawer, including all of its fields and labels.
	/// </summary>
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return (fieldHeight + fieldPaddingY) * amountOfFieldsInProperty;
	}

	private void SetIndent(out int currentIndent, int desiredIndent)
	{
		currentIndent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = desiredIndent;
	}
}