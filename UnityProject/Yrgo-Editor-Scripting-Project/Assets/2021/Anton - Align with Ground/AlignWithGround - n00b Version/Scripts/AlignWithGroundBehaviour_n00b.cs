using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithGroundBehaviour_n00b : MonoBehaviour
{
	public void DropChildObjects(Transform parentTransform = null)
	{
		if (parentTransform == null)
			parentTransform = this.transform;

		foreach (Transform child in parentTransform)
		{
#if UNITY_EDITOR
			UnityEditor.Undo.RecordObject(child, $"Align With Ground: Ground Object '{child.name}'");
#endif
			Ground(child);
		}
	}

	public void Redo()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.PerformRedo();
#endif
	}

	public void Undo()
	{
#if UNITY_EDITOR
		UnityEditor.Undo.PerformUndo();
#endif
	}

	private void Ground(Transform child)
	{
		Ray ray = new Ray(child.transform.position, Vector3.down);
		RaycastHit hitInfo;
		Physics.Raycast(ray, out hitInfo);

		if (hitInfo.distance < 100.0f)
		{
			child.transform.position = hitInfo.point;

			if (child.transform.position == Vector3.zero)
			{
				Debug.Log(child.transform.name + "was placed out of bounds, now at 0,0,0");
			}
		}
	}
}