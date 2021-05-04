/*
 * Copyright (c) The Knights of Unity
 * http://theknightsofunity.com/
 */

using UnityEditor;
using UnityEngine;

public class CheatsWindow : EditorWindow
{
    [MenuItem("Examples/The Knights of Unity/Cheats")]
    public static void ShowWindow()
    {
        GetWindow<CheatsWindow>(false, "Cheats", true);
    }

    void OnGUI()
    {
        Cheats.MuteAllSounds = EditorGUILayout.Toggle("Mute All Sounds", Cheats.MuteAllSounds);
        Cheats.PlayerLifes = EditorGUILayout.IntField("Player Lifes", Cheats.PlayerLifes);
        Cheats.PlayerTwoName = EditorGUILayout.TextField("Player Two Name", Cheats.PlayerTwoName);

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(30)))
        {
            Cheats.MuteAllSounds = false;
            Cheats.PlayerLifes = 4;
            Cheats.PlayerTwoName = "John";
        }
        EditorGUILayout.EndHorizontal();
    }
}