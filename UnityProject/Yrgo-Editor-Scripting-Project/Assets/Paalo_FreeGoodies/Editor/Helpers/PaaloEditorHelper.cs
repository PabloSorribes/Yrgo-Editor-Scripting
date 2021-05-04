using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.EditorTools
{
	/// <summary>
	/// Contains helper methods for common (and tedious) operations when writing IMGUI-code.
	/// </summary>
	public static class PaaloEditorHelper
	{
		private static readonly float originalLabelWidth = EditorGUIUtility.labelWidth;
		private static readonly int originalIndentation = EditorGUI.indentLevel;

		/// <summary>
		/// Button which is disabled (greyed out) depending on the bool which is sent in.
		/// <para></para>
		/// Only handles 
		/// </summary>
		/// <param name="buttonText">The text displayed in the button.</param>
		/// <param name="onClickCallback">What action/method to perform when the button is clicked</param>
		/// <param name="buttonColor">Optional color your button should have</param>
		/// <param name="isActive">Enable/Disable the button depending on conditions you define</param>
		/// <param name="addVerticalBox">Add a box around the button for prettyfication</param>
		public static void ButtonDisableable(string buttonText, System.Action onClickCallback, bool isActive = true, Color? buttonColor = null, bool addVerticalBox = true)
		{
			//Make it possible to make the button greyed out and "inactive".
			bool oldGuiEnabled = GUI.enabled;
			GUI.enabled = isActive;

			//Set color and check if it is null
			var oldGuiColor = GUI.color;
			GUI.color = buttonColor.GetValueOrDefault(oldGuiColor);

			//Create box for prettyfication
			if (addVerticalBox)
				EditorGUILayout.BeginVertical(GUI.skin.box);

			//Perform action if clicked
			if (GUILayout.Button(buttonText))
			{
				onClickCallback?.Invoke();
			}

			//End prettyfication-box
			if (addVerticalBox)
				EditorGUILayout.EndVertical();

			//Reset GUI to old values
			GUI.color = oldGuiColor;
			GUI.enabled = oldGuiEnabled;
		}

		/// <summary>
		/// Button which can be hidden depending on bool sent in.
		/// </summary>
		/// <param name="buttonText"></param>
		/// <param name="onClick"></param>
		/// <param name="buttonColor"></param>
		/// <param name="isVisible"></param>
		public static void ButtonHideable(string buttonText, System.Action onClick, bool isVisible = true, Color? buttonColor = null, bool addVerticalBox = true)
		{
			if (!isVisible)
			{
				return;
			}

			//Set color
			var oldGuiColor = GUI.color;
			GUI.color = buttonColor.GetValueOrDefault(oldGuiColor);

			//Create box for prettyfication
			if (addVerticalBox)
				EditorGUILayout.BeginVertical(GUI.skin.box);

			//Perform action if clicked
			if (GUILayout.Button(buttonText))
			{
				onClick?.Invoke();
			}

			//End prettyfication-box
			if (addVerticalBox)
				EditorGUILayout.EndVertical();

			//Reset GUI to old values
			GUI.color = oldGuiColor;
		}

		/// <summary>
		/// A GUIStyle made of a box which is shaped like line.
		/// </summary>
		/// <returns></returns>
		public static GUIStyle GetSeparatorMarginStyle()
		{
			var separatorMarginStyle = new GUIStyle(GUI.skin.box) { margin = new RectOffset(0, 0, 10, 10) };
			return separatorMarginStyle;
		}

		/// <summary>
		/// Draws a box/rect which looks like a separation line. Useful for sectioning off different parts of your GUI.
		/// <para></para>
		/// Expands its width by default to match the width of the GUI/Editor Window.
		/// </summary>
		public static void MakeSeparatorLine(bool expandWidth = true)
		{
			var separatorMarginStyle = GetSeparatorMarginStyle();
			GUILayout.Box("", separatorMarginStyle, GUILayout.Height(1f), GUILayout.ExpandWidth(expandWidth));
		}

		public static void IncrementIndentation()
		{
			EditorGUI.indentLevel++;
		}
		public static void DecrementIndentation()
		{
			EditorGUI.indentLevel--;
		}

		public static void SetIndentation(int desiredIndent)
		{
			EditorGUI.indentLevel = desiredIndent;
		}

		public static void ResetIndentation()
		{
			EditorGUI.indentLevel = originalIndentation;
		}

		/// <summary>
		/// Useful for making a Bool Toggle (<see cref="EditorGUILayout.Toggle(bool, GUILayoutOption[])"/>) have its checkbox at the far right of an editor window. 
		/// </summary>
		/// <param name="containingBoxForLabel"></param>
		/// <param name="offsetFromRightEdge"></param>
		public static void SetWideLabelWidth(Rect containingBoxForLabel, float offsetFromRightEdge = 30f)
		{
			//Make all the longer folder paths visible, as well as make all checkboxes align along the right side of the box.
			var checkBoxPadding = offsetFromRightEdge;
			var alignedCheckBoxesLabelWidth = containingBoxForLabel.width - checkBoxPadding;
			EditorGUIUtility.labelWidth = alignedCheckBoxesLabelWidth;
		}

		/// <summary>
		/// Reset the <see cref="EditorGUIUtility.labelWidth"/> to the default value.
		/// </summary>
		public static void ResetLabelWidth()
		{
			EditorGUIUtility.labelWidth = originalLabelWidth;
		}

		/// <summary>
		/// Useful for setting the <see cref="EditorGUIUtility.labelWidth"/> to the width of your desired label.
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public static float CalculateLabelWidth(GUIContent label, float padding = 0f)
		{
			float labelWidth = GUI.skin.label.CalcSize(label).x + padding;
			return labelWidth;
		}

		/// <summary>
		/// Useful for setting the <see cref="EditorGUIUtility.labelWidth"/> to the width of your desired label.
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public static float CalculateLabelWidth(string label, float padding = 0f)
		{
			return CalculateLabelWidth(new GUIContent(label), padding);
		}


		#region ProgressBar Handling
		//Source: https://github.com/Unity-Technologies/VFXToolbox/blob/master/Editor/Utility/VFXToolboxGUIUtility.cs

		private static double s_LastProgressBarTime;

		/// <summary>
		/// Displays a progress bar with delay and optional cancel button
		/// </summary>
		/// <param name="title">title of the window</param>
		/// <param name="message">message</param>
		/// <param name="progress">progress</param>
		/// <param name="delay">minimum delay before displaying window</param>
		/// <param name="cancelable">is the window cancellable?</param>
		/// <returns>true if cancelled, false otherwise</returns>
		public static bool DisplayProgressBar(string title, string message, float progress, float delay = 0.0f, bool cancelable = false)
		{
			if (s_LastProgressBarTime < 0.0)
				s_LastProgressBarTime = EditorApplication.timeSinceStartup;

			if (EditorApplication.timeSinceStartup - s_LastProgressBarTime > delay)
			{
				if (cancelable)
				{
					return EditorUtility.DisplayCancelableProgressBar(title, message, progress);
				}
				else
				{
					EditorUtility.DisplayProgressBar(title, message, progress);
					return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Clears the current progressbar
		/// </summary>
		public static void ClearProgressBar()
		{
			s_LastProgressBarTime = -1.0;
			EditorUtility.ClearProgressBar();
		}

		#endregion

		#region Other GUI Utils
		//Source: https://github.com/Unity-Technologies/VFXToolbox/blob/master/Editor/Utility/VFXToolboxGUIUtility.cs
		public static void GUIRotatedLabel(Rect position, string label, float angle, GUIStyle style)
		{
			var matrix = GUI.matrix;
			var rect = new Rect(position.x - 10f, position.y, position.width, position.height);
			GUIUtility.RotateAroundPivot(angle, rect.center);
			GUI.Label(rect, label, style);
			GUI.matrix = matrix;
		}
		#endregion

		#region Draw Drag And Drop Area

		/// <summary>
		/// Draws a Drag and Drop Area and allows you to send in a method which receives an array of the objects that were dragged into the area.
		/// <para></para>
		/// The caller method needs to receive a generic type "T" and then cast it to its desired type itself OR use a Lambda Expression for handling the OnDragged-event.
		/// <para></para>
		/// Example implementation in OnGUI: '<see cref="Paalo.UnityMiscTools.Examples.DragAndDropAreaExample"/>'
		/// </summary>
		/// <seealso cref="HowToDrawDragAndDropArea"/>
		/// <typeparam name="T">The object type you want the '<paramref name="OnPerformedDragCallback"/>'-method to handle.</typeparam>
		/// <param name="dragAreaInfo">How the DragArea should look like and what text it should display.</param>
		/// <returns></returns>
		public static void DrawDragAndDropArea<T>(DragAndDropAreaInfo dragAreaInfo, System.Action<T[]> OnPerformedDragCallback = null) where T : UnityEngine.Object
		{
			//Change color and create Drag Area
			Color originalGUIColor = GUI.color;
			GUI.color = dragAreaInfo.outlineColor;
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = dragAreaInfo.backgroundColor;
			var dragArea = GUILayoutUtility.GetRect(dragAreaInfo.dragAreaWidth, dragAreaInfo.dragAreaHeight, GUILayout.ExpandWidth(true));
			GUI.Box(dragArea, dragAreaInfo.DragAreaText);

			//See if the current Editor Event is a DragAndDrop event.
			var anEvent = Event.current;
			switch (anEvent.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (!dragArea.Contains(anEvent.mousePosition))
					{
						//Early Out in case the drop is made outside the drag area.
						break;
					}

					//Change mouse cursor icon to the "Copy" icon
					DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

					//If mouse is released 
					if (anEvent.type == EventType.DragPerform)
					{
						DragAndDrop.AcceptDrag();
						var draggedTypeObjectsArray = GetDraggedObjects<T>();
						OnPerformedDragCallback?.Invoke(draggedTypeObjectsArray);
					}

					Event.current.Use();
					break;
			}

			EditorGUILayout.EndVertical();
			GUI.color = originalGUIColor;
		}

		private static T[] GetDraggedObjects<T>() where T : UnityEngine.Object
		{
			List<T> draggedTypeObjects = new List<T>();

			foreach (var draggedObject in DragAndDrop.objectReferences)
			{
				//A "DefaultAsset" is a folder in the Unity Editor.
				if (draggedObject is DefaultAsset)
				{
					string folderPath = AssetDatabase.GetAssetPath(draggedObject);
					var assetsInDraggedFolders = GetAllAssetsOfTypeInDirectory<T>(folderPath);
					foreach (var asset in assetsInDraggedFolders)
					{
						if (draggedTypeObjects.Contains(asset as T))
						{
							//Debug.Log($"Asset in Dragged Folder exists already: '{asset.name}'");
							continue;
						}

						draggedTypeObjects.Add(asset as T);
					}
					//Go to next index in the "DragAndDrop.objectReferences"
					continue;
				}

				//Dragged asset is a "normal" asset, ie. not a Folder.
				T draggedAsset = draggedObject as T;
				if (draggedAsset == null || draggedTypeObjects.Contains(draggedAsset as T))
				{
					//Debug.Log($"Dragged Asset is not casteable to the type you wanted or already exists in the selection list: '{draggedAsset.name}'");
					continue;
				}

				//Debug.Log($"Asset of type '{draggedAsset.GetType().FullName}' dragged. Asset Name: '{draggedAsset.name}'");
				draggedTypeObjects.Add(draggedAsset as T);
			}
			return draggedTypeObjects.ToArray();
		}

		#endregion Draw Drag And Drop Area

		public static void DrawHeader(string headerText, params GUILayoutOption[] layoutOptions)
		{
			EditorGUILayout.LabelField(headerText, EditorStyles.boldLabel, layoutOptions);
		}

		/// <summary>
		/// Draw a scrollable TextArea which displays all the elements of the array you send in as clickable and copy-pasteable text.
		/// <para></para>
		/// See '<see cref="Examples.DrawTextAreaExample.GUI_DrawTextAreaScroller"/>' on how to call this method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="typeArray">An array of objects deriving from <see cref="UnityEngine.Object"/> that you want to display the elements of.</param>
		public static void DrawTextAreaScroller<T>(T[] typeArray) where T : UnityEngine.Object
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);

			//Label for amount of Target DSGs
			EditorGUILayout.LabelField($"Added '{typeof(T).Name}'-objects: {typeArray?.Length}", EditorStyles.boldLabel);

			//Making a long string with a newline between each element.
			string arrayObjectNamesString = string.Empty;
			if (typeArray != null)
			{
				foreach (var typeObj in typeArray)
				{
					if (typeObj == null)
						continue;

					arrayObjectNamesString += $"{typeObj?.name}\n";
				}
				arrayObjectNamesString.TrimEnd(System.Environment.NewLine.ToCharArray());
			}

			//Only show TextArea if any elements have been added.
			if (typeArray != null && typeArray.Length > 0)
			{
				//Initialize variables for displaying the Scrollable TextArea
				string textAreaString = "";
				Vector2 textAreaScroller = Vector2.zero;

				//Calculate the size of the text area by adding one lineHeight per element 
				textAreaScroller = EditorGUILayout.BeginScrollView(textAreaScroller);
				float textAreaSingleLineHeight = EditorGUIUtility.singleLineHeight - 1f;
				float textAreaHeight = textAreaSingleLineHeight * typeArray.Length;

				//Make text area that shows what items the array contains.
				textAreaString = EditorGUILayout.TextArea(arrayObjectNamesString, GUILayout.Height(textAreaHeight));
				EditorGUILayout.EndScrollView();
			}

			EditorGUILayout.EndVertical();
		}

		/// <summary>
		/// Loads all assets of a certain type defined by the <paramref name="pattern"/> (the extension of the asset).
		/// </summary>
		/// <param name="path">Path relative to the project, eg. "Assets/Game/Prefabs"</param>
		/// <param name="pattern">*.prefab, *.scene, etc</param>
		/// <returns></returns>
		public static GameObject[] LoadAssetsFromPath(string path, string pattern)
		{
			var result = new List<GameObject>();
			var info = new DirectoryInfo(path);
			var fileInfo = info.GetFiles(pattern);

			foreach (var file in fileInfo)
				result.Add(AssetDatabase.LoadAssetAtPath<GameObject>($"{path}/{file.Name}"));

			return result.ToArray();
		}

		public static T[] GetAllAssetsOfTypeInDirectory<T>(string path) where T : UnityEngine.Object
		{
			List<T> assetsToGet = new List<T>();

			string absolutePath = $"{Application.dataPath}/{path.Remove(0, 7)}";
			string[] fileEntries = Directory.GetFiles(absolutePath);

			foreach (string fileName in fileEntries)
			{
				string sanitizedFileName = fileName.Replace('\\', '/');
				int index = sanitizedFileName.LastIndexOf('/');
				string localPath = path;
				if (index > 0)
				{
					localPath += sanitizedFileName.Substring(index);
				}

				T assetOfType = AssetDatabase.LoadAssetAtPath<T>(localPath);
				if (assetOfType != null)
					assetsToGet.Add(assetOfType);
			}
			return assetsToGet.ToArray();
		}

		/// <summary>
		/// Open a folder panel which let's you browse to any folder on disk.
		/// </summary>
		/// <param name="startPath"></param>
		/// <returns>The path to the folder that the user selected</returns>
		public static string BrowseToFolder(string startPath = "Assets")
		{
			string folderPath = EditorUtility.OpenFolderPanel("Browse to Folder", startPath, "");
			if (string.IsNullOrEmpty(folderPath))
			{
				//Cancelled the OpenFolderPanel window.
				return string.Empty;
			}

			if (folderPath.Contains(Application.dataPath))
			{
				folderPath = folderPath.Replace(Application.dataPath, string.Empty).Trim();
				folderPath = folderPath.TrimStart('/', '\\');
				folderPath = $"Assets/{folderPath}";
			}
			return folderPath;
		}

		/// <summary>
		/// Returns the assets that are selected in the Project Tab.
		/// </summary>
		/// <returns></returns>
		public static Object[] GetSelectedAssetsInProjectView()
		{
			Object[] selectedAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
			return selectedAssets;
		}

		/// <summary>
		/// Returns a filtered selection of the selected assets which have the type '<typeparamref name="T"/>' on them (eg. a prefab, but not an actual script asset).
		/// </summary>
		/// <returns></returns>
		public static Object[] GetSelectedAssetsInProjectView<T>() where T : UnityEngine.Object
		{
			Object[] selectedAssets = Selection.GetFiltered(typeof(T), SelectionMode.Assets);
			return selectedAssets;
		}

		/// <summary>
		/// Find all prefabs containing a specific component (T)
		/// <para>Source: http://answers.unity.com/answers/734018/view.html </para>
		/// </summary>
		/// <typeparam name="T">The type of component</typeparam>
		public static List<GameObject> LoadPrefabsContaining<T>(string path) where T : UnityEngine.Component
		{
			List<GameObject> result = new List<GameObject>();

			var allFiles = Resources.LoadAll<UnityEngine.Object>(path);
			foreach (var obj in allFiles)
			{
				if (obj is GameObject)
				{
					GameObject go = obj as GameObject;
					if (go.GetComponent<T>() != null)
					{
						result.Add(go);
					}
				}
			}
			return result;
		}
	}
}
