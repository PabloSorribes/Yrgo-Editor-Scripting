using System;
using System.Linq;
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

		// Source: https://stackoverflow.com/a/4405876
		public static string CapitalizeFirstLetter(this string input)
		{
			switch (input)
			{
				case null: throw new ArgumentNullException(nameof(input));
				case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
				default: return input.First().ToString().ToUpper() + input.Substring(1);
			}
		}

		// Source: https://stackoverflow.com/a/4335913
		public static string TrimStart(this string target, string trimString)
		{
			if (string.IsNullOrEmpty(trimString)) return target;

			string result = target;
			while (result.StartsWith(trimString))
			{
				result = result.Substring(trimString.Length);
			}

			return result;
		}

		// Source: https://stackoverflow.com/a/4335913
		public static string TrimEnd(this string target, string trimString)
		{
			if (string.IsNullOrEmpty(trimString)) return target;

			string result = target;
			while (result.EndsWith(trimString))
			{
				result = result.Substring(0, result.Length - trimString.Length);
			}

			return result;
		}
	}
}
