using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.Examples
{
	public class DrawTextAreaExample : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "DrawTextAreaExample";

		[MenuItem(CurrentPackageConstants.packageExamplesMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		public static void ToolsMenu()
		{
			SetupWindow();
		}

		public static void SetupWindow()
		{
			var window = GetWindow<DrawTextAreaExample>(true, toolName, true);
			window.minSize = new Vector2(300, 300);
			window.maxSize = new Vector2(window.minSize.x + 100, window.minSize.y + 100);
		}
		#endregion ToolName and SetupWindow  

		//Used to be able to use serializedProperty for displaying the 'arrayObjects'-array
		SerializedObject serializedEditorWindowObject;

		//This array can be of any type that derives from UnityEngine.Object (eg. GameObject, AnimationClip, etc)
		[SerializeField] AudioClip[] arrayObjects = new AudioClip[0];

		private void OnEnable()
		{
			serializedEditorWindowObject = new SerializedObject(this);
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI_DrawArray();
			EditorGUILayout.Space();
			GUI_DrawTextAreaScroller();
			EditorGUILayout.EndVertical();
		}

		private void GUI_DrawArray()
		{
			serializedEditorWindowObject.Update();

			var arrayProperty = serializedEditorWindowObject.FindProperty(nameof(arrayObjects));
			EditorGUILayout.PropertyField(arrayProperty, new GUIContent($"'{arrayObjects.GetType().Name}'-array"));

			serializedEditorWindowObject.ApplyModifiedProperties();
		}

		private void GUI_DrawTextAreaScroller()
		{
			Paalo.UnityMiscTools.EditorTools.PaaloEditorHelper.DrawTextAreaScroller(arrayObjects);
		}
	}
}