using _Project.Scripts.Enemies.AI;
using _Project.Scripts.Enemies.ScriptableObjects;
using _Project.Scripts.Player;
using _Project.Scripts.Player.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using PlayerSettings = _Project.Scripts.Player.ScriptableObjects.PlayerSettings;

namespace _Project.Scripts.Editor
{
	public class MySettingsWindow : EditorWindow
	{
		[MenuItem("Window/SettingsOverview")]
		private static void ShowWindow()
		{
			window = GetWindow<MySettingsWindow>();
			window.titleContent = new GUIContent("SettingsOverview");
			window.Show();
		}

		private static MySettingsWindow window;
		
		private readonly string[] _toolbarNames  = {"BaseSettings", "PlayerElements", "EnemyElements"};
		private          int      _selectedToolbar = 0;
		
		public PlayerSettings         playerSettings;
		public EnemySettings          enemySettings;
		public PlayerElementalStats[] playerElementalStats;
		public EnemyElementalStats[]  enemyElementalStats;
		
		public UnityEditor.Editor[] editors;

		public PlayerController playerController;

		public Vector2 scrollPosition;
		public bool    liveUpdate;

		private bool _playmodeHasInitialized;
		private int  _undoBeforePlaymodeReference;

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
			DrawWindow();

			EditorGUILayout.EndScrollView();
		}

		private void LoadAssets()
		{
			playerSettings = Resources.Load<PlayerSettings>("PlayerSettings/PlayerBaseSettings");
			enemySettings  = Resources.Load<EnemySettings>("EnemySettings/EnemyBaseSettings");
			
			playerElementalStats = new [] 
			{
				Resources.Load<PlayerElementalStats>("PlayerElementalStats/EarthStats"),
				Resources.Load<PlayerElementalStats>("PlayerElementalStats/WindStats"),
				Resources.Load<PlayerElementalStats>("PlayerElementalStats/WaterStats"),
				Resources.Load<PlayerElementalStats>("PlayerElementalStats/FireStats"),
			};
			
			enemyElementalStats = new [] 
			{
				Resources.Load<EnemyElementalStats>("EnemyElementalStats/EarthStats"),
				Resources.Load<EnemyElementalStats>("EnemyElementalStats/WindStats"),
				Resources.Load<EnemyElementalStats>("EnemyElementalStats/WaterStats"),
				Resources.Load<EnemyElementalStats>("EnemyElementalStats/FireStats"),
			};
		}

		private void PlayModeInit()
		{
			if (playerController == null)
				playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			
			_undoBeforePlaymodeReference = Undo.GetCurrentGroup();
			_playmodeHasInitialized      = true;
		}
		
		private void UpdateStats()
		{
			if (playerController == null)
				playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			
			playerController.UpdateStatsFromEditorWindow();
			
			var enemies = FindObjectsOfType<EnemyController>(false);
			foreach (var enemy in enemies)
				enemy.SetStatsFromEditorWindow();
		}
		
		private void SetEditorsBasedOnToolbar()
		{
			editors = _selectedToolbar switch 
			{
				0 => new[] 
				{
					UnityEditor.Editor.CreateEditor(playerSettings),
					UnityEditor.Editor.CreateEditor(enemySettings),
				},
				
				1 => new[] 
				{
					UnityEditor.Editor.CreateEditor(playerElementalStats[0]),
					UnityEditor.Editor.CreateEditor(playerElementalStats[1]),
					UnityEditor.Editor.CreateEditor(playerElementalStats[2]),
					UnityEditor.Editor.CreateEditor(playerElementalStats[3]),
				},
				
				2 => new[] 
				{
					UnityEditor.Editor.CreateEditor(enemyElementalStats[0]),
					UnityEditor.Editor.CreateEditor(enemyElementalStats[1]),
					UnityEditor.Editor.CreateEditor(enemyElementalStats[2]),
					UnityEditor.Editor.CreateEditor(enemyElementalStats[3]),
				},
				
				_ => editors
			};
		}
		
		private void DrawWindow()
		{
			GUILayout.BeginVertical();
			
			EditorGUILayout.Space(6);
			liveUpdate = EditorGUILayout.Toggle("Update Changes Live", liveUpdate);
			EditorGUILayout.Space(6);

			var style = new GUIStyle(GUI.skin.label) {fontSize = 14};
			foreach (var editor in editors)
			{
				EditorGUILayout.LabelField($"-- {editor.target.name} --", style);
				EditorGUILayout.Space(2);
				editor.OnInspectorGUI();
				EditorGUILayout.Space(15);
			}
			
			GUILayout.EndVertical();
		}
		
		private void OnQuitting()
		{
			if (EditorUtility.DisplayDialog(title:   "Information",
											message: "Changes to SettingsOverview that was made during playmode can be saved if you wish.\r\n\r\n" +
											         "Do you want to keep the changes?",
											ok:      "Yes",
											cancel:  "No"))
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