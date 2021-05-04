using Paalo.UnityMiscTools.EditorTools;
using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.Examples
{
	public class ExampleWindow : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "Example Window";

		[MenuItem(CurrentPackageConstants.packageRightClickMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		public static void RightClickMenu()
		{
			SetupWindow();
		}

		[MenuItem(CurrentPackageConstants.packageWindowMenuPath + toolName, false, CurrentPackageConstants.packageMenuIndexPosition)]
		public static void ToolsMenu()
		{
			SetupWindow();
		}

		public static void SetupWindow()
		{
			var window = GetWindow<ExampleWindow>(true, toolName, true);
			window.minSize = new Vector2(300, 200);
			window.maxSize = new Vector2(window.minSize.x + 100, window.minSize.y + 100);
		}
		#endregion ToolName and SetupWindow

		private void OnGUI()
		{
			GUISection_SetAudioClips();
		}

		private void GUISection_SetAudioClips()
		{
			//Disable button if no gameobjects are selected.
			var selectedObjects = Selection.gameObjects;
			bool enableGui = selectedObjects.Length > 0 ? true : false;

			PaaloEditorHelper.ButtonDisableable("Disableable Button with no Extras", () => { Debug.Log("No Extras"); });

			PaaloEditorHelper.ButtonDisableable($"Selected '{selectedObjects.Length}' GameObjects, say 'Hej!'",
				() => { Debug.Log("Hej!"); },
				enableGui,
				null,
				false);
		}
	}
}
