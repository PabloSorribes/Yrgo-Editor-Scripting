using System;
using System.Collections.Generic;
using UnityEngine;

public class LambdaComparer<T> : IComparer<T>
{
	public Func<T, T, int> Comparer { get; set; }

	public int Compare(T x, T y)
	{
		return Compare(x, y);
	}

	private void ExampleImplementation()
	{
		List<Transform> transforms = new List<Transform>();

		transforms.Sort(new LambdaComparer<Transform>()
		{
			Comparer = (x, y) =>
			{
				if (x.position.x > y.position.x)
				{
					return 1;
				}
				else if (x.position.x < y.position.x)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			}
		});
	}
}
