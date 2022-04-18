using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(AlignWithGroundBehaviour_Pro))]
public class AlignWithGroundEditor_Pro : Editor
{
	public override void OnInspectorGUI()
	{
		// --- TWO WAYS OF DRAWING THE DEFAULT INSPECTOR --- //

		// 1: Calling the base-function that you're overriding. 
		// This will probably also call serializedObject.Update() for you
		base.OnInspectorGUI();

		// 2: Calling "DrawDefaultInspector()", which seems to do the same thing. 
		// DrawDefaultInspector();

		// --- GET A REFERENCE TO YOUR BEHAVIOUR-SCRIPT --- //
		// Cast the built in "target"-variable to your script type (ie. the Type you're doing a custom inspector for).
		// Save it in a variable that you can use to get its children from etc.
		AlignWithGroundBehaviour_Pro script = (AlignWithGroundBehaviour_Pro)target;


		// --- DRAW YOUR BUTTONS AND PERFORM YOUR CUSTOM ACTION --- // 
		// Draw the Ground Object Button
		if (GUILayout.Button("Ground Object"))
		{
			// - Access the children of your Custom Inspector's gameObject.
			//  (This works cause "Transform" implements the IEnumarable-interface, which allows you to use the "foreach"-statement.)
			// - Record an Undo-step for each child. Unity will combine them into one big Undo-step later on.
			// - Run the function for sending each child to the ground.
			foreach (Transform child in script.transform)
			{
				Undo.RecordObject(child, $"Align With Ground: Ground Object '{child.name}'");
				SendToGround(child);
			}
		}

		// GUILayout has a "Space"-function in which you can specify the space yourself.
		GUILayout.Space(20f);

		// Showcasing that you can call Undo/Redo yourself in Editor Code, if you like.
		if (GUILayout.Button("Perform Undo"))
		{
			Undo.PerformUndo();
		}

		// EditorGUILayout ALSO has a "Space"-function but here you CANNOT specify the space yourself (lol).
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