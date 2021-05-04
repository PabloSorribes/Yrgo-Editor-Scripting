//Written by Pablo Sorribes Bernhard, 2019 02 16

using UnityEngine;

/// <summary>
/// Showcases how the <see cref="MinMaxSlider"/> can be used in a normal Monobehaviour without implementing a completely new Custom Inspector-class.
/// <para></para>
/// The PropertyDrawer draws itself the way you want it to whenever that class is used as a variable somewhere.
/// </summary>
public class MinMaxSliderExample : MonoBehaviour
{
	public MinMaxSlider minMaxSlider1;

	[Space(20)]
	public float normalFloat;
	[Range(0,1)]
	public float normalSlider;

	[Space(60)]
	public float normalFloat2;
	[Range(0, 1)]
	public float normalSlider2;
	public MinMaxSlider minMaxSlider2;


	private void OnGUI()
	{
		if (GUILayout.Button($"{nameof(minMaxSlider1)} - GetRandomValueWithinValueRange"))
		{
			float randomNumber = minMaxSlider1.GetRandomValueWithinValueRange();
			Debug.Log($"Random number within MinVal and MaxVal: {randomNumber}");
		}
	}
}