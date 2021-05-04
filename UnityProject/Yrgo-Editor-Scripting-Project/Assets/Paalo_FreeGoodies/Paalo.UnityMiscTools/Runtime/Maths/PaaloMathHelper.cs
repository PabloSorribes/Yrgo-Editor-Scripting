namespace Paalo.UnityMiscTools
{
	public static class PaaloMathHelper
	{
		/// <summary>
		/// Remaps a value (x) in interval [A,B], to the proportional value in interval [C,D]
		/// </summary>
		/// <param name="x">The value to remap.</param>
		/// <param name="A">the minimum bound of interval [A,B] that contains the x value</param>
		/// <param name="B">the maximum bound of interval [A,B] that contains the x value</param>
		/// <param name="C">the minimum bound of target interval [C,D]</param>
		/// <param name="D">the maximum bound of target interval [C,D]</param>
		public static float Remap(float x, float A, float B, float C, float D)
		{
			float remappedValue = C + (x-A)/(B-A) * (D - C);
			return remappedValue;
		}

		/// <summary>
		/// Instead of using <see cref="UnityEngine.Mathf.Approximately(float, float)"/> for zero checking. 
		/// This is a bit faster.
		/// </summary>
		/// <param name="valueToCheck"></param>
		/// <returns></returns>
		public static bool IsApproximatelyZero(float valueToCheck)
		{
			if (valueToCheck > float.Epsilon)
			{
				return false;
			}
			return true;
		}
	}
}