using UnityEngine;

namespace Paalo.UnityMiscTools.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Removes any '<paramref name="subStringToRemove"/>'-string from the string this extension method is called from.
		/// </summary>
		/// <param name="self"></param>
		/// <param name="subStringToRemove"></param>
		/// <returns></returns>
		public static string StripSubString(this string self, string subStringToRemove)
		{
			if (self.Contains(subStringToRemove))
			{
				return self.Replace(subStringToRemove, string.Empty).Trim();
			}
			Debug.LogWarning($"There was no matching string '{subStringToRemove}' inside of '{self}'.\n" +
				$"Will return '{self}' instead.");
			return self;
		}
	}
}
