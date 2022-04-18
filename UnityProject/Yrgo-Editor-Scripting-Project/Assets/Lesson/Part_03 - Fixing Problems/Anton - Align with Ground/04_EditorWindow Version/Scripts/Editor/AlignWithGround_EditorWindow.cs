using UnityEditor;
using UnityEngine;

public class AlignWithGround_EditorWindow : EditorWindow
{
	private const string menuItemPath = "YRGO/Part 03/" + myWindowTitle;
	private const string myWindowTitle = "AlignWithGround_EditorWindow";

	[MenuItem(menuItemPath)]
	public static void SetupWindow()
	{
		//Instantiate the window and set its size.
		var window = GetWindow<AlignWithGround_EditorWindow>(utility: false, title: myWindowTitle, focus: true);
		window.minSize = new Vector2(400, 175);
		window.maxSize = new Vector2(window.minSize.x + 10, window.minSize.y + 10);
	}

	public void OnGUI()
	{
		// --- DRAW YOUR BUTTONS AND PERFORM YOUR CUSTOM ACTION --- //
		//Draw the Ground Object Button
		if (GUILayout.Button($"Ground {Selection.transforms.Length} Objects"))
		{
			// - Record an Undo-step for each selected object. Unity will combine them into one big Undo-step later on.
			// - Run the function for sending each child to the ground.
			foreach (Transform trans in Selection.transforms)
			{
				Undo.RecordObject(trans, $"Align With Ground: Ground Object '{trans.name}'");
				SendToGround(trans);
			}
		}

		//GUILayout has a "Space"-function in which you can specify the space yourself.
		GUILayout.Space(20f);

		//Showcasing that you can call Undo/Redo yourself in Editor Code, if you like.
		if (GUILayout.Button("Perform Undo"))
		{
			Undo.PerformUndo();
		}

		//EditorGUILayout ALSO has a "Space"-function but here you CANNOT specify the space yourself (lol).
		EditorGUILayout.Space();

		if (GUILayout.Button("Perform Redo"))
		{
			Undo.PerformRedo();
		}
	}

	/// <summary>
	/// Raycasts downwards 100f. If it hits something --> move the current child to the raycastHit's position.
	/// <para></para>
	/// Code by Anton Lindkvist (aka "Sjupp#5217")
	/// </summary>
	/// <param name="child"></param>
	private void SendToGround(Transform child, float maxCheckDistance = 100f)
	{
		Ray ray = new Ray(child.transform.position, Vector3.down);
		RaycastHit hitInfo;
		Physics.Raycast(ray, out hitInfo);

		if (hitInfo.distance < maxCheckDistance)
		{
			child.transform.position = hitInfo.point;

			if (child.transform.position == Vector3.zero)
			{
				Debug.Log(child.transform.name + "was placed out of bounds, now at 0,0,0");
			}
		}
	}
}