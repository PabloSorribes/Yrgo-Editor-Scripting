using RSG.Scene.Query;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Order objects from top to bottom of the hierarchy view.
/// </summary>
public class SceneGraphOrderComparer : IComparer<GameObject>
{
	public int Compare(GameObject x, GameObject y)
	{
		var xParent = x.Parent();
		var yParent = y.Parent();

		var xSiblingIndex = x.transform.GetSiblingIndex();
		var ySiblingIndex = y.transform.GetSiblingIndex();

		//Har samma parent eller null (root objects) från början → jämför sibling index → early out.
		if (xParent == yParent)
		{
			if (xSiblingIndex > ySiblingIndex)
			{
				return 1;
			}
			else if (xSiblingIndex < ySiblingIndex)
			{
				return -1;
			}
			else
			{
				return 0;
			}
		}

		//Tillhör samma gren, ta reda på om man är en child till "Contains(x/y)"
		//Om man är en child så har man större värde
		if (x.Ancestors().Contains(y))
		{
			return 1;
		}
		else if (y.Ancestors().Contains(x))
		{
			return -1;
		}

		var xDepth = x.Ancestors().Count();
		var yDepth = y.Ancestors().Count();

		//Temporary variables, to be able to overwrite them later and keep the input variables intact.
		var xCopy = x;
		var yCopy = y;

		//Om man är på samma djup så kan man dela parent, annars går det inte. 
		//Hitta parents på samma djup för att kunna jämföra SiblingIndex.
		//Set parents to same depth
		if (xDepth > yDepth)
		{
			while (xCopy.Ancestors().Count() > yDepth)
			{
				xCopy = xCopy.Parent();
			}
		}
		else if (xDepth < yDepth)
		{
			while (xDepth < yCopy.Ancestors().Count())
			{
				yCopy = yCopy.Parent();
			}
		}

		//Backa uppåt tills båda har samma parent (eller parent == null)
		//Förutsätter att båda är på samma djup.
		while (xCopy.Parent() != yCopy.Parent())
		{
			xCopy = xCopy.Parent();
			yCopy = yCopy.Parent();
		}

		//Both copies should have the same parent (or parent is null), thus we can make a comparison of their sibling index.
		if (xCopy.transform.GetSiblingIndex() > yCopy.transform.GetSiblingIndex())
		{
			return 1;
		}
		else if (xCopy.transform.GetSiblingIndex() < yCopy.transform.GetSiblingIndex())
		{
			return -1;
		}
		return 0;
	}
}