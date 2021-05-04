//Source: https://forum.unity.com/threads/drawing-a-field-using-multiple-property-drawers.479377/#post-3304795

using System.Reflection;
using UnityEditor;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class VisibleIf : MultiPropertyAttribute
{
	public string MethodName { get; private set; }
	public bool InvertVisibilityToggle { get; private set; }

	private MethodInfo eventMethodInfo = null;
	private PropertyInfo eventPropertyInfo = null;
	private FieldInfo fieldInfo = null;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="methodName">The method or field which should be shown/hidden</param>
	/// <param name="invertVisibility">FALSE == if the target bool is TRUE, show this field/method. TRUE = if the target bool is TRUE, hide this field/method</param>
	public VisibleIf(string methodName, bool invertVisibility = false)
	{
		this.MethodName = methodName;
		this.InvertVisibilityToggle = invertVisibility;
	}

	public override bool IsVisible(SerializedProperty property)
	{
		return Visibility(property) != InvertVisibilityToggle;
	}

	private bool Visibility(SerializedProperty property)
	{
		System.Type eventOwnerType = property.serializedObject.targetObject.GetType();
		string eventName = MethodName;

		// Try finding a method with the name provided:
		if (eventMethodInfo == null)
			eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

		if (eventPropertyInfo == null)
			eventPropertyInfo = eventOwnerType.GetProperty(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

		// If we could not find a method with that name, look for a field:
		if (eventMethodInfo == null && fieldInfo == null)
			fieldInfo = eventOwnerType.GetField(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

		if (eventMethodInfo != null)
			return (bool)eventMethodInfo.Invoke(property.serializedObject.targetObject, null);
		else if (eventPropertyInfo != null)
			return (bool)eventPropertyInfo.GetValue(property.serializedObject.targetObject);
		else if (fieldInfo != null)
			return (bool)fieldInfo.GetValue(property.serializedObject.targetObject);
		else
			Debug.LogWarning(string.Format($"VisibleIf: Unable to find method, property or field {eventName} in {eventOwnerType}"));

		return true;
	}
}