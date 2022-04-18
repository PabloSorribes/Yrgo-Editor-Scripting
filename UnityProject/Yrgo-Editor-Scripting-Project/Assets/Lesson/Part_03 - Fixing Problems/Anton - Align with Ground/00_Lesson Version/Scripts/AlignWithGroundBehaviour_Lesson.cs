using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithGroundBehaviour_Lesson : MonoBehaviour
{
	Vector3[] locations;

	public void DropChildObjects()
	{
		locations = new Vector3[transform.childCount];
		int i = 0;
		foreach (Transform child in transform)
		{
			locations[i] = child.transform.position;
			Ground(child);
			i++;
		}
	}

	public void Undo()
	{
		if (locations != null)
		{
			int i = 0;
			foreach (Transform child in transform)
			{
				child.transform.position = locations[i];
				i++;
			}
			locations = null;
		}
		else
		{
			Debug.Log("Nothing to undo");
		}
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