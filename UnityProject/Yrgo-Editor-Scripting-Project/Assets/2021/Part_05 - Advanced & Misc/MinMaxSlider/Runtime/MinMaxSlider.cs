//Written by Pablo Sorribes Bernhard, 2019 02 16

/// <summary>
/// Reference this class to get a MinMaxSlider in the inspector,
/// allowing you to set your custom ranges and get the respective min/max values set in the slider.
/// <para></para>
/// Has a custom property drawer, hence no Tooltip Attributes and similar will work on it.
/// </summary>
[System.Serializable]
public class MinMaxSlider
{
	public float sliderRangeMin = 0.1f;
	public float sliderRangeMax = 10f;

	public float minVal = 2f;
	public float maxVal = 8f;

	/// <summary>
	/// A random value between '<see cref="minVal"/>' and '<see cref="maxVal"/>'.
	/// </summary>
	/// <returns></returns>
	public float GetRandomValueWithinValueRange()
	{
		float randomNumber = UnityEngine.Random.Range(minVal, maxVal);
		return randomNumber;
	}

	/// <summary>
	/// A random value between '<see cref="sliderRangeMin"/>' and '<see cref="sliderRangeMax"/>'.
	/// </summary>
	/// <returns></returns>
	public float GetRandomValueWithinTotalRange()
	{
		float randomNumber = UnityEngine.Random.Range(sliderRangeMin, sliderRangeMax);
		return randomNumber;
	}
}
