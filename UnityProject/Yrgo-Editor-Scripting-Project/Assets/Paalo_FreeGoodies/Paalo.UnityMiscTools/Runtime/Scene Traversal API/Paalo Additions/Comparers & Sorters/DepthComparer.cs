using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The GameObject with the most ancestors (ie. the Deepest Child) will be sorted as later in the list.
/// </summary>
public class DepthComparer : IComparer<GameObject>
{
	public int Compare(GameObject x, GameObject y)
	{
		if (Ancestors(x).Count() > Ancestors(y).Count())
		{
			return 1;
		}
		else if (Ancestors(x).Count() < Ancestors(y).Count())
		{
			return -1;
		}
		else
		{
			return 0;
		}
	}

	/// <summary>
	/// Get collection of all ancestors of a particular game object, starting with the immediate parent and working up to the root object.
	/// </summary>
	private IEnumerable<GameObject> Ancestors(GameObject parent)
	{
		var ancestorTransform = parent.transform.parent;
		while (ancestorTransform != null)
		{
			yield return ancestorTransform.gameObject;
			ancestorTransform = ancestorTransform.parent;
		}
	}
}