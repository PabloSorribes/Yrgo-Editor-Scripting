using UnityEditor;
using UnityEngine;
using SettingsOverviewWindowExample.Data;
using System.Collections.Generic;

namespace SettingsOverviewWindowExample.Editor
{
	public class SettingsOverviewWindow : EditorWindow
	{
		[MenuItem("YRGO/Part 05/Settings Overview Window")]
		private static void ShowWindow()
		{
			window = GetWindow<SettingsOverviewWindow>();
			window.titleContent = new GUIContent("SettingsOverview");
			window.Show();
		}

		private static SettingsOverviewWindow window;

		private readonly string[] _toolbarNames = {
			"BaseSettings",
			"PlayerStats",
			"EnemyStats"
		};
		private int _selectedToolbar = 0;


		public SettingsOverviewWindowExample.Data.PlayerStats playerStats;
		public SettingsOverviewWindowExample.Data.EnemyStats enemyStats;
		public SettingsOverviewWindowExample.Data.OtherStats[] otherStatsArray;

		public UnityEditor.Editor[] editors;

		//public SettingsOverviewWindowExample.Runtime.PlayerController playerController;

		public Vector2 scrollPosition;
		public bool liveUpdate;

		private bool _playmodeHasInitialized;
		private int _undoBeforePlaymodeReference;

		private void OnEnable()
		{
			Application.quitting += OnQuitting;
			LoadAssets();
		}

		private void OnGUI()
		{
			if (Application.isPlaying && !_playmodeHasInitialized)
				PlayModeInit();

			if (Application.isPlaying && liveUpdate)
				UpdateStats();

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false);

			GUILayout.BeginHorizontal();
			_selectedToolbar = GUILayout.Toolbar(_selectedToolbar, _toolbarNames);
			GUILayout.EndHorizontal();

			SetEditorsBasedOnToolbar();
			GUI_DrawWindow();

			EditorGUILayout.EndScrollView();
		}

		private void LoadAssets()
		{
			playerStats = Resources.Load<PlayerStats>("SettingsOverview/Player/PlayerStats");
			enemyStats = Resources.Load<EnemyStats>("SettingsOverview/Enemy/EnemyStats");

			string searchPath = "Assets";
			var filePaths = GetAllFilePathsAtFolder("t:OtherStats", searchPath);
		
			List<OtherStats> otherStatsList = new List<OtherStats>();
			foreach (var path in filePaths)
			{
				var otherStatAsset = AssetDatabase.LoadAssetAtPath<OtherStats>(path);
				otherStatsList.Add(otherStatAsset);
			}
			otherStatsArray = otherStatsList.ToArray();

			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			builder.AppendLine($"SearchPath: {searchPath}");

			builder.AppendLine("Filepaths:");
			foreach (var path in filePaths)
			{
				builder.AppendLine(path);
				builder.AppendLine();
			}

			Debug.Log(builder.ToString());
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

		private void PlayModeInit()
		{
			//if (playerController == null)
			//	playerController = GameObject.FindWithTag("Player").GetComponent<SettingsOverviewWindowExample.Runtime.PlayerController>();

			_undoBeforePlaymodeReference = Undo.GetCurrentGroup();
			_playmodeHasInitialized = true;
		}

		private void UpdateStats()
		{
			//if (playerController == null)
			//	playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

			//playerController.UpdateStatsFromEditorWindow();

			//var enemies = FindObjectsOfType<EnemyController>(false);
			//foreach (var enemy in enemies)
			//	enemy.SetStatsFromEditorWindow();
		}

		private void SetEditorsBasedOnToolbar()
		{
			editors = _selectedToolbar switch
			{
				0 => new[]
				{
					UnityEditor.Editor.CreateEditor(playerStats),
					UnityEditor.Editor.CreateEditor(enemyStats),
				},

				1 => new[]
				{
					UnityEditor.Editor.CreateEditor(playerStats),
				},

				2 =>  GetOtherStatsEditors(),

				_ => editors
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

			EditorGUILayout.Space(6);
			liveUpdate = EditorGUILayout.Toggle("Update Changes Live", liveUpdate);
			EditorGUILayout.Space(6);

			var style = new GUIStyle(GUI.skin.label) { fontSize = 14 };
			foreach (var editor in editors)
			{
				EditorGUILayout.LabelField($"-- {editor.target.name} --", style);
				EditorGUILayout.Space(2);

				// HERE we draw the default inspector for the object!
				editor.OnInspectorGUI();	

				EditorGUILayout.Space(15);
			}

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