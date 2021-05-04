using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.EditorTools
{
	public class SetAudioClipsUtility : EditorWindow
	{
		#region ToolName and SetupWindow
		private const string toolName = "Set AudioClips Utility";

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
			var window = GetWindow<SetAudioClipsUtility>(true, toolName, true);
			window.minSize = new Vector2(300, 200);
			window.maxSize = new Vector2(window.minSize.x + 100, window.minSize.y + 100);
		}
		#endregion ToolName and SetupWindow

		public string startPath = "Assets/Game/Audio/Source";
		public AudioClip[] audioClips = new AudioClip[0];

		public string textArea = "";
		Vector2 textAreaScroller;

		private void OnGUI()
		{
			GUISection_GetAudioClips();
			EditorGUILayout.Space();
			GUISection_SetAudioClips();
			EditorGUILayout.Space();
			GUISection_ShowSelectedAudioClipsTextArea();
		}

		private void UpdateAudioClipsOnDragAndDrop<T>(T[] draggedObjects) where T : UnityEngine.Object
		{
			audioClips = draggedObjects as AudioClip[];
		}

		private void GUISection_GetAudioClips()
		{
			Color oldGuiColor = GUI.color;
			EditorGUILayout.BeginVertical(GUI.skin.box);

			EditorGUILayout.Space();
			var dragAndDropInfo = new DragAndDropAreaInfo("Audio Clips", Color.black, Color.cyan);
			PaaloEditorHelper.DrawDragAndDropArea<AudioClip>(dragAndDropInfo, UpdateAudioClipsOnDragAndDrop);
			EditorGUILayout.Space();

			GUI.color = Color.red;
			if (GUILayout.Button("Clear selected AudioClips"))
			{
				//audioClips = null;
				audioClips = new AudioClip[0];
			}
			EditorGUILayout.Space();

			GUI.color = oldGuiColor;
			EditorGUILayout.EndVertical();
		}

		private void GUISection_ShowSelectedAudioClipsTextArea()
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);

			//Label for amount of Audio Clips
			EditorGUILayout.LabelField($"Selected Audio Clips: {audioClips?.Length}", EditorStyles.boldLabel);

			//Making a long string with a newline between each element.
			string selectedClipNamesString = "";
			if (audioClips != null)
			{
				foreach (var clip in audioClips)
				{
					selectedClipNamesString += $"{clip.name}\n";
				}
				selectedClipNamesString.TrimEnd(System.Environment.NewLine.ToCharArray());
			}

			//Just show TextArea if any audio clips have been selected.
			if (audioClips != null && audioClips.Length > 0)
			{
				//Make text area showing what clips have been selected already.
				//Calculate the size of the text area by adding one lineHeight per element 
				textAreaScroller = EditorGUILayout.BeginScrollView(textAreaScroller);
				float textAreaSingleLineHeight = EditorGUIUtility.singleLineHeight - 1f;
				float textAreaHeight = textAreaSingleLineHeight * audioClips.Length;
				textArea = EditorGUILayout.TextArea(selectedClipNamesString, GUILayout.Height(textAreaHeight));
				EditorGUILayout.EndScrollView();
			}

			EditorGUILayout.EndVertical();
		}

		private void GUISection_SetAudioClips()
		{
			bool oldGUIEnabled = GUI.enabled;
			Color oldGuiColor = GUI.color;
			var selectedObjects = Selection.gameObjects;

			//Disable button if no clips are selected.
			GUI.enabled = audioClips.Length > 0 ? true : false;

			//Draw Button
			EditorGUILayout.BeginVertical(GUI.skin.box);
			GUI.color = Color.cyan;
			if (GUILayout.Button($"Apply {audioClips.Length} AudioClips to {selectedObjects.Length} selected GameObjects!"))
			{
				SetAudioClips(audioClips, selectedObjects);
			}
			EditorGUILayout.EndVertical();

			GUI.color = oldGuiColor;
			GUI.enabled = oldGUIEnabled;
		}

		private static void SetAudioClips(AudioClip[] audioClips, GameObject[] gameObjects)
		{
			//Sort the selection in the hierarchy order, top to bottom.
			List<GameObject> gameObjectsList = new List<GameObject>(gameObjects);
			gameObjectsList.Sort(new SceneGraphOrderComparer());

			//Find the gameObjects which have an AudioSource on them.
			IEnumerable<GameObject> gameObjectsWithAudioSources = gameObjectsList.Where(go => go.GetComponent<AudioSource>() != null);
			gameObjectsList = gameObjectsWithAudioSources.ToList();

			//Loop through the gameObjects with AudioSources and apply clips to them.
			//Also loop back and continue from the first clip if there are more AudioSources selected than AudioClips available.
			for (int i = 0; i < gameObjectsList.ToArray().Length; i++)
			{
				AudioSource audioSource = gameObjectsList[i].GetComponent<AudioSource>();
				Undo.RecordObject(audioSource, $"Set AudioClip to {audioSource.name}");

				// When you have less AudioClips than selected GameObjects, this will start the AudioClip-iteration again.
				// Source: https://stackoverflow.com/a/27624122/11027794
				int clipIndexWithinAudioArrayRange = i % audioClips.Length;
				audioSource.clip = audioClips[clipIndexWithinAudioArrayRange];
			}

			Debug.Log($"Applied AudioClips to {gameObjectsList.ToArray().Length} GameObjects with AudioSources.");
		}




		private void GUISection_OldGetClipsButton()
		{
			Color oldGuiColor = GUI.color;
			EditorGUILayout.Space();
			startPath = EditorGUILayout.TextField("Starting Path: ", startPath);
			EditorGUILayout.Space();

			GUI.color = Color.cyan;
			if (GUILayout.Button($"Gimme them Audio Clips!"))
			{
				string directoryToBrowse = PaaloEditorHelper.BrowseToFolder(startPath);
				if (string.IsNullOrEmpty(directoryToBrowse))
				{
					return;
				}

				audioClips = PaaloEditorHelper.GetAllAssetsOfTypeInDirectory<AudioClip>(directoryToBrowse);
			}
			GUI.color = oldGuiColor;
		}
	}
}
