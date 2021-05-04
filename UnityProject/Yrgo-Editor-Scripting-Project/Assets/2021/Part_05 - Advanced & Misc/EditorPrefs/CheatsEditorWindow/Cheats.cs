/*
 * Copyright (c) The Knights of Unity
 * http://theknightsofunity.com/
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Cheats
{
    public static bool MuteAllSounds
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetBool("MuteAllSounds", false);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetBool("MuteAllSounds", value);
#endif
        }
    }

    public static int PlayerLifes
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetInt("PlayerLifes", 3);
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetInt("PlayerLifes", value);
#endif
        }
    }

    public static string PlayerTwoName
    {
        get
        {
#if UNITY_EDITOR
            return EditorPrefs.GetString("PlayerTwoName", "John");
#else
            return false;
#endif
        }

        set
        {
#if UNITY_EDITOR
            EditorPrefs.SetString("PlayerTwoName", value);
#endif
        }
    }
}