//Source: https://forum.unity.com/threads/drawing-a-field-using-multiple-property-drawers.479377/#post-3304795

using System.Linq;
using UnityEditor;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public abstract class MultiPropertyAttribute : PropertyAttribute
{
	public IOrderedEnumerable<object> stored = null;

	public virtual void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PropertyField(position, property, label);
	}

	public virtual void OnPreGUI(Rect position, SerializedProperty property) { }
	public virtual void OnPostGUI(Rect position, SerializedProperty property) { }

	public virtual bool IsVisible(SerializedProperty property) { return true; }
	public virtual float? GetPropertyHeight(SerializedProperty property, GUIContent label) { return null; }
}