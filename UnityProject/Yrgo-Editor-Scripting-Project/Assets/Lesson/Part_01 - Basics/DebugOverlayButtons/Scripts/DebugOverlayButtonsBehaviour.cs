using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugOverlayButtonsBehaviour : MonoBehaviour
{
	private void OnGUI()
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		if (GUILayout.Button("Test Button"))
		{
			Debug.Log($"Test Buttons was clicked!");
		}
#endif
	}
}
