//Source: http://answers.unity.com/answers/1641578/view.html
//Also shows how to show lists and arrays

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.Examples
{
	public class CustomEditorForEditorWindow : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "CustomEditorForEditorWindow";

		[MenuItem(CurrentPackageConstants.packageExamplesMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		public static void ToolsMenu() => SetupWindow();

		public static void SetupWindow() { GetWindow<CustomEditorForEditorWindow>(toolName, true); }
		#endregion ToolName and SetupWindow

		Editor editor;
		[SerializeField] List<MyClass> myClassListTest = new List<MyClass>();

		void OnGUI()
		{
			if (!editor)
				editor = Editor.CreateEditor(this);

			if (editor)
				editor.OnInspectorGUI();
		}

		void OnInspectorUpdate() => Repaint();
	}

	[System.Serializable]
	public class MyClass
	{
		public string myString;
		public int myInt;
		public List<int> myIntList;
	}

	[CustomEditor(typeof(CustomEditorForEditorWindow), true)]
	public class ListTestEditorDrawer : Editor
	{
		public override void OnInspectorGUI()
		{
			var list = serializedObject.FindProperty("myClassListTest");
			EditorGUILayout.PropertyField(list, new GUIContent("My List Test"), true);
		}
	}
}