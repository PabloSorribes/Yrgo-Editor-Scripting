using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class MiscHowTo_EditorScripting
{
	public static void SetActiveSceneDirty()
	{
		var activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
		UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
	}
}
