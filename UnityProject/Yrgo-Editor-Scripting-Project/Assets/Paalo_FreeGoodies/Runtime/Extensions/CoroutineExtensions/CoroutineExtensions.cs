using System.Collections;
using UnityEngine;

namespace Paalo.UnityMiscTools.Extensions
{
	public static class CoroutineExtensions
	{
		//Usage example
		public static CoroutineController StartCoroutineEx(this MonoBehaviour monoBehaviour, IEnumerator routine)
		{
			if (routine == null)
			{
				throw new System.ArgumentNullException("routine");
			}

			CoroutineController coroutineController = new CoroutineController(routine);
			coroutineController.StartCoroutine(monoBehaviour);
			return coroutineController;
		}
	}
}