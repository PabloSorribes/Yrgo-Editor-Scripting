//Shows how to show lists and arrays in an EditorWindow

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.Examples
{
	/// <summary>
	/// Example of how to make an editor window which can easily draw arrays/lists.
	/// <para></para>
	/// The trick is to make a <see cref="SerializedObject"/> based on the <see cref="EditorWindow"/>
	/// and use <see cref="SerializedObject.FindProperty(string)"/> to create the array fields.
	/// </summary>
	public class EditorWindowDrawArray : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "EditorWindowDrawArray";

		[MenuItem(CurrentPackageConstants.packageExamplesMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		public static void ToolsMenu() => SetupWindow();

		public static void SetupWindow() { GetWindow<EditorWindowDrawArray>(toolName, true); }
		#endregion ToolName and SetupWindow

		SerializedObject serializedEditorWindowObject;

		[SerializeField] MonoScript[] arrayTest = new MonoScript[0];
		[SerializeField] List<MonoScript> listTest = new List<MonoScript>();

		private void OnEnable()
		{
			serializedEditorWindowObject = new SerializedObject(this);
		}

		void OnGUI()
		{
			serializedEditorWindowObject.Update();

			var arrayProperty = serializedEditorWindowObject.FindProperty(nameof(arrayTest));
			EditorGUILayout.PropertyField(arrayProperty);

			EditorGUILayout.Space();

			var listProperty = serializedEditorWindowObject.FindProperty(nameof(listTest));
			EditorGUILayout.PropertyField(listProperty);

			serializedEditorWindowObject.ApplyModifiedProperties();
		}
	}
}