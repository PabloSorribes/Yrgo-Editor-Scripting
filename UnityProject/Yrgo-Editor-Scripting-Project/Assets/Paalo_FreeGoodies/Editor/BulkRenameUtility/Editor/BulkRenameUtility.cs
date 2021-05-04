using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.EditorTools
{
	public class BulkRenameUtility : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "Bulk Rename Utility";

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
			var window = GetWindow<BulkRenameUtility>(true, toolName, true);
			window.minSize = new Vector2(350, 375);
			window.maxSize = new Vector2(window.minSize.x, window.minSize.y);
		}
		#endregion ToolName and SetupWindow

		public string addString = "";
		public bool addStringAsPrefix = false;

		public string stringToRemove = "";
		public string replacementString = "";

		public string setNewNameString = "";

		public bool addNumberAsPrefix = false;
		public bool topToBottom = true;

		private void OnGUI()
		{
			GUISection_SetString();
			EditorGUILayout.Space();
			GUISection_ReplaceString();
			EditorGUILayout.Space();
			GUISection_AddString();
			EditorGUILayout.Space();
			GUISection_AddNumber();
		}

		private void GUISection_SetString()
		{
			var selectedObjects = Selection.gameObjects;
			EditorGUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.LabelField("Rename GameObjects", EditorStyles.boldLabel);
			setNewNameString = EditorGUILayout.TextField("New Name: ", setNewNameString);

			if (GUILayout.Button($"Rename {selectedObjects.Length} selected objects to '{setNewNameString}'."))
			{
				SetString(setNewNameString, selectedObjects);
			}
			EditorGUILayout.EndVertical();
		}

		private void GUISection_ReplaceString()
		{
			var selectedObjects = Selection.gameObjects;
			EditorGUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.LabelField("Find and Replace", EditorStyles.boldLabel);
			stringToRemove = EditorGUILayout.TextField("String to Replace: ", stringToRemove);
			replacementString = EditorGUILayout.TextField("Replace with: ", replacementString);

			if (GUILayout.Button($"Replace '{stringToRemove}' with '{replacementString}' on {selectedObjects.Length} selected objects."))
			{
				ReplaceString(stringToRemove, replacementString, selectedObjects);
			}
			EditorGUILayout.EndVertical();
		}

		private void GUISection_AddString()
		{
			var selectedObjects = Selection.gameObjects;
			EditorGUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.LabelField("Add string suffix/prefix to name", EditorStyles.boldLabel);
			addString = EditorGUILayout.TextField("String to Add: ", addString);

			GUIContent prefixToggleLabel = new GUIContent("Add as Prefix: ", "TRUE = Add the string as Prefix. FALSE = Add the string as Suffix.");
			addStringAsPrefix = EditorGUILayout.Toggle(prefixToggleLabel, addStringAsPrefix);

			if (GUILayout.Button($"Add '{addString}' to {selectedObjects.Length} selected objects."))
			{
				AddString(addString, addStringAsPrefix, selectedObjects);
			}
			EditorGUILayout.EndVertical();
		}

		private void GUISection_AddNumber()
		{
			var selectedObjects = Selection.gameObjects;
			EditorGUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.LabelField("Add number as suffix/prefix to name", EditorStyles.boldLabel);

			GUIContent prefixToggleLabel = new GUIContent("Add as Prefix: ", "TRUE = Add the number as Prefix. FALSE = Add the number as Suffix.");
			addNumberAsPrefix = EditorGUILayout.Toggle(prefixToggleLabel, addNumberAsPrefix);

			GUIContent topToBottomToggleLabel = new GUIContent("Top to Bottom: ", "TRUE = Numbers start from the top object in the selection. FALSE = Numbers start from the bottom and up.");
			topToBottom = EditorGUILayout.Toggle(topToBottomToggleLabel, topToBottom);

			string suffixVsPrefix = "suffix";
			if (addNumberAsPrefix)
			{
				suffixVsPrefix = "prefix";
			}

			if (GUILayout.Button($"Add number-{suffixVsPrefix} to {selectedObjects.Length} selected objects."))
			{
				AddNumber(addNumberAsPrefix, topToBottom, selectedObjects);
			}
			EditorGUILayout.EndVertical();
		}

		public static void SetString(string setNewNameString, GameObject[] gameObjects)
		{
			for (int i = 0; i < gameObjects.Length; i++)
			{
				var currentObject = gameObjects[i];
				Undo.RecordObject(currentObject, $"Rename '{setNewNameString}' to {currentObject.name}");
				currentObject.name = setNewNameString;
			}
		}

		private static void ReplaceString(string stringToRemove, string replacementString, GameObject[] gameObjects)
		{
			if (string.IsNullOrEmpty(stringToRemove))
			{
				//Debug.LogWarning("You have to add a value to the remove string.");
				return;
			}

			for (int i = 0; i < gameObjects.Length; i++)
			{
				var currentObject = gameObjects[i];
				Undo.RecordObject(currentObject, $"Replace '{stringToRemove}' from {currentObject.name} with '{replacementString}'");

				string objName = currentObject.name;
				if (objName.Contains(stringToRemove))
				{
					objName = objName.Replace(stringToRemove, replacementString).Trim();
					currentObject.name = objName;
				}
			}
		}

		public static void AddString(string stringToAdd, bool addAsPrefix, GameObject[] gameObjects)
		{
			for (int i = 0; i < gameObjects.Length; i++)
			{
				var currentObject = gameObjects[i];
				Undo.RecordObject(currentObject, $"Add '{stringToAdd}' to {currentObject.name}");

				string objName = currentObject.name;
				if (addAsPrefix)
				{
					currentObject.name = $"{stringToAdd}{objName}";
				}
				else
				{
					currentObject.name = $"{objName}{stringToAdd}";
				}
			}
		}

		public static void AddNumber(bool addAsPrefix, bool topToBottom, GameObject[] gameObjects)
		{
			List<GameObject> gameObjectsList = new List<GameObject>(gameObjects);
			gameObjectsList.Sort(new SceneGraphOrderComparer());

			if (!topToBottom)
			{
				gameObjectsList.Reverse();
			}

			for (int i = 0; i < gameObjectsList.ToArray().Length; i++)
			{
				var currentObject = gameObjectsList[i];
				Undo.RecordObject(currentObject, $"Add number to {currentObject.name}");

				string objName = currentObject.name;
				string stringToAdd;

				if (i < 9)
				{
					stringToAdd = $"0{i + 1}";
				}
				else
				{
					stringToAdd = $"{i + 1}";
				}

				if (addAsPrefix)
				{
					currentObject.name = $"{stringToAdd} {objName}";
				}
				else
				{
					currentObject.name = $"{objName} {stringToAdd}";
				}
			}
		}

	}
}
