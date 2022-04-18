// Original script made by Linus Jonsson, GP20, 2021
// Edited and cleaned up by Pablo Sorribes Bernhard, 2022-04-19

using UnityEditor;
using UnityEngine;
using SettingsOverviewWindowExample.Data;
using System.Collections.Generic;

namespace SettingsOverviewWindowExample.Editor
{
	public class SettingsOverviewWindow : EditorWindow
	{
		/// <summary>
		/// Used to get the titleContent for the Window Title in the GUI-code
		/// </summary>
		private static SettingsOverviewWindow window;

		[MenuItem("YRGO/Part 05/Settings Overview Window")]
		private static void ShowWindow()
		{
			window = GetWindow<SettingsOverviewWindow>();
			window.titleContent = new GUIContent("Settings Overview Window");
			window.Show();
		}

		private readonly string[] _toolbarNames = {
			"PlayerStats",
			"EnemyStats",
			"OtherStatsArray"
		};
		private int _selectedToolbar = 0;


		private SettingsOverviewWindowExample.Data.PlayerStats playerStats;
		private SettingsOverviewWindowExample.Data.EnemyStats enemyStats;
		private SettingsOverviewWindowExample.Data.OtherStats[] otherStatsArray;

		private UnityEditor.Editor[] editorsToDraw;

		private Vector2 scrollPosition;

		private bool _playmodeHasInitialized;
		private int _undoBeforePlaymodeReference;

		private void OnEnable()
		{
			Application.quitting += OnQuitting;
			LoadAssets();
		}

		private void OnDestroy()
		{
			// Avoid getting multiple callbacks if the window is closed and reopened several times
			Application.quitting -= OnQuitting;
		}

		private void LoadAssets()
		{
			playerStats = Resources.Load<PlayerStats>("SettingsOverview/Player/PlayerStats");
			enemyStats = Resources.Load<EnemyStats>("SettingsOverview/Enemy/EnemyStats");

			// How to search the entire "Assets"-folder after a specific type of asset, and load that one instead:
			string searchPath = "Assets";
			var filePaths = GetAllFilePathsAtFolder("t:OtherStats", searchPath);

			List<OtherStats> otherStatsList = new List<OtherStats>();
			foreach (var path in filePaths)
			{
				var otherStatAsset = AssetDatabase.LoadAssetAtPath<OtherStats>(path);
				otherStatsList.Add(otherStatAsset);
			}
			otherStatsArray = otherStatsList.ToArray();

			// Debugging the files we got from the search:
			//System.Text.StringBuilder builder = new System.Text.StringBuilder();
			//builder.AppendLine($"SearchPath: {searchPath}");
			//
			//builder.AppendLine("Filepaths:");
			//foreach (var path in filePaths)
			//{
			//	builder.AppendLine(path);
			//	builder.AppendLine();
			//}
			//
			//Debug.Log(builder.ToString());
		}

		/// <summary>
		/// Should NOT have a trailing forward slash, else it won't work 
		/// <para></para>
		/// eg. "TopFolder/SubFolder" = correct, "TopFolder/SubFolder/" = incorrect
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="folderPath">Should NOT have a trailing forward slash, else it won't work (eg. "TopFolder/SubFolder" = correct, "TopFolder/SubFolder/" = incorrect)</param>
		/// <returns></returns>
		private static string[] GetAllFilePathsAtFolder(string filter, string folderPath)
		{
			List<string> filePaths = new List<string>();

			// search for a ScriptObject called CatchphraseCategoryData inside the PhraseDataFolder
			var guids = AssetDatabase.FindAssets($"{filter}", new string[] { folderPath });
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				filePaths.Add(path);
			}

			return filePaths.ToArray();
		}

		private void OnGUI()
		{
			if (Application.isPlaying && !_playmodeHasInitialized)
				PlayModeInit();

			GUI_DrawWindow();
		}

		private void PlayModeInit()
		{
			_playmodeHasInitialized = true;
			_undoBeforePlaymodeReference = Undo.GetCurrentGroup();
		}

		private void SetEditorsBasedOnToolbar()
		{
			editorsToDraw = _selectedToolbar switch
			{
				0 => new[]
				{
					UnityEditor.Editor.CreateEditor(playerStats),
				},

				1 => new[]
				{
					UnityEditor.Editor.CreateEditor(enemyStats),
				},

				2 => GetOtherStatsEditors(),

				_ => editorsToDraw
			};
		}

		private UnityEditor.Editor[] GetOtherStatsEditors()
		{
			List<UnityEditor.Editor> editors = new List<UnityEditor.Editor>();

			foreach (var statAsset in otherStatsArray)
			{
				var statAssetEditor = UnityEditor.Editor.CreateEditor(statAsset);
				editors.Add(statAssetEditor);
			}
			return editors.ToArray();
		}

		private void GUI_DrawWindow()
		{
			GUILayout.BeginVertical();

			// Draw the Window Title at the top of the script
			EditorGUILayout.Space(6);
			var windowTitleStyle = new GUIStyle(GUI.skin.label) {
				fontSize = 18,
				clipping = TextClipping.Overflow,
				fontStyle = FontStyle.Bold
			};
			EditorGUILayout.LabelField($"{window.titleContent}", windowTitleStyle);
			EditorGUILayout.Space(6);

			// Draw toolbar and update which tab should be active
			GUILayout.BeginHorizontal();
			_selectedToolbar = GUILayout.Toolbar(_selectedToolbar, _toolbarNames);
			GUILayout.EndHorizontal();
			SetEditorsBasedOnToolbar();

			// Make the window Scrollable, if enough elements are displayed.
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);

			// Draw the current tab's inspectors
			var objectHeaderStyle = new GUIStyle(GUI.skin.label) { fontSize = 14 };
			foreach (var editor in editorsToDraw)
			{
				EditorGUILayout.LabelField($"-- {editor.target.name} --", objectHeaderStyle);
				EditorGUILayout.Space(2);

				// HERE we draw the default inspector for the object!
				editor.OnInspectorGUI();

				EditorGUILayout.Space(15);
			}

			// Tell Unity to end the Scrollbar here
			EditorGUILayout.EndScrollView();

			GUILayout.EndVertical();
		}

		private void OnQuitting()
		{
			if (EditorUtility.DisplayDialog(title: "Information",
											message: "Changes to SettingsOverview that was made during playmode can be saved if you wish.\r\n\r\n" +
													 "Do you want to keep the changes?",
											ok: "Yes",
											cancel: "No"))
			{
				Debug.Log("OverviewSettings: Changes have been saved.");
			}
			else
			{
				Undo.RevertAllDownToGroup(_undoBeforePlaymodeReference);
			}

			_playmodeHasInitialized = false;
		}
	}
}