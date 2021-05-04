//Source: https://forum.unity.com/threads/drawing-a-field-using-multiple-property-drawers.479377/#post-3304795

using UnityEditor;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class Indent : MultiPropertyAttribute
{
	public override void OnPreGUI(Rect position, SerializedProperty property)
	{
		EditorGUI.indentLevel++;
	}
	public override void OnPostGUI(Rect position, SerializedProperty property)
	{
		EditorGUI.indentLevel--;
	}
}