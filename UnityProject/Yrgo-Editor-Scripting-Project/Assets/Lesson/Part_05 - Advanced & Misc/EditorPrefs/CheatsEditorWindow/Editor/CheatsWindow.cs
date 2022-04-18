/*
 * Copyright (c) The Knights of Unity
 * http://theknightsofunity.com/
 */

using UnityEditor;
using UnityEngine;

public class CheatsWindow : EditorWindow
{
    [MenuItem("YRGO/Part 05/The Knights of Unity/Cheats")]
    public static void ShowWindow()
    {
        GetWindow<CheatsWindow>(false, "Cheats", true);
    }

    void OnGUI()
    {
        Cheats.MuteAllSounds = EditorGUILayout.Toggle("Mute All Sounds", Cheats.MuteAllSounds);
        Cheats.PlayerLives = EditorGUILayout.IntField("Player Lives", Cheats.PlayerLives);
        Cheats.PlayerTwoName = EditorGUILayout.TextField("Player Two Name", Cheats.PlayerTwoName);

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(30)))
        {
            Cheats.MuteAllSounds = false;
            Cheats.PlayerLives = 4;
            Cheats.PlayerTwoName = "John";
        }
        EditorGUILayout.EndHorizontal();
    }
}