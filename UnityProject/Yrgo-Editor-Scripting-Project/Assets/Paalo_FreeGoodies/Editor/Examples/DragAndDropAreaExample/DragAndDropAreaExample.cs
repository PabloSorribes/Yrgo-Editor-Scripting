using UnityEditor;
using UnityEngine;
using Paalo.UnityMiscTools.EditorTools;

namespace Paalo.UnityMiscTools.Examples
{
	public class DragAndDropAreaExample : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "DragAndDropAreaExample";

		[MenuItem(CurrentPackageConstants.packageExamplesMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		public static void ToolsMenu()
		{
			SetupWindow();
		}

		public static void SetupWindow()
		{
			var window = GetWindow<DragAndDropAreaExample>(true, toolName, true);
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
			DrawDragAndDropArea_LambdaExpression();
			EditorGUILayout.Space();
			DrawDragAndDropArea_NormalMethod();
			EditorGUILayout.Space();
			GUI_DrawTextAreaScroller();
			EditorGUILayout.EndVertical();
		}

		/// <summary>
		/// Example method on how to call the '<see cref="PaaloEditorHelper.DrawDragAndDropArea{T}(DragAndDropAreaInfo, System.Action{T[]})"/>' 
		/// from OnGUI() using a Lambda Expression to handle the dragged objects.
		/// </summary>
		private void DrawDragAndDropArea_LambdaExpression()
		{
			EditorGUILayout.LabelField("Lambda Expression-method:", EditorStyles.boldLabel);

			//Using a Lambda Expression for the Callback
			PaaloEditorHelper.DrawDragAndDropArea<AudioClip>(
				new DragAndDropAreaInfo("AudioClips"),
				draggedObjects =>
				{
					Debug.Log($"Dragged Objects Length: {draggedObjects.Length}");
					arrayObjects = draggedObjects;
				});
		}

		/// <summary>
		/// Example method on how to call the '<see cref="PaaloEditorHelper.DrawDragAndDropArea{T}(DragAndDropAreaInfo, System.Action{T[]})"/>' 
		/// from OnGUI(), using a "dedicated" callback function.
		/// </summary>
		private void DrawDragAndDropArea_NormalMethod()
		{
			EditorGUILayout.LabelField("Normal Callback-method:", EditorStyles.boldLabel);

			//Using a "proper" function to handle the Callback
			PaaloEditorHelper.DrawDragAndDropArea<AudioClip>(
				new DragAndDropAreaInfo("AudioClips"),
				OnDragAndDropPerformed_CallbackExample);
		}

		/// <summary>
		/// Example method on how to handle the objects that are received through the OnPerformedDragCallback in the '<see cref="DrawDragAndDropArea_NormalMethod"/>'-method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="draggedObjects"></param>
		private void OnDragAndDropPerformed_CallbackExample<T>(T[] draggedObjects) where T : UnityEngine.Object
		{
			var myObjects = draggedObjects as AudioClip[];

			Debug.Log("Dragged Object Array Length: " + myObjects.Length);
			Debug.Log($"Dragged Obj Array Type: {draggedObjects.GetType().FullName}");
			foreach (var draggedObj in draggedObjects)
			{
				Debug.Log($"Dragged Obj Type: {draggedObj.GetType().FullName}");
			}

			arrayObjects = myObjects;
		}

		/// <summary>
		/// Displays the elements dragged into the '<see cref="arrayObjects"/>'-array
		/// </summary>
		private void GUI_DrawTextAreaScroller()
		{
			Paalo.UnityMiscTools.EditorTools.PaaloEditorHelper.DrawTextAreaScroller(arrayObjects);
		}
	}
}