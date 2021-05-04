//Source: https://forum.unity.com/threads/drawing-a-field-using-multiple-property-drawers.479377/#post-3304795

using UnityEngine;

public class TestScript_UsingMultiplePropertyAttributes : MonoBehaviour
{
	public bool showMonkeys;
	[Indent]
	[VisibleIf(nameof(showMonkeys), invertVisibility: false)]
	public int monkeyCount = 5;

	[Space(10f)]
	public bool hideElephants;
	[Indent]
	[VisibleIf(nameof(hideElephants), invertVisibility: true)]
	public int elephantCount = 10;


	[Range(0, 10)]
	public int gimmeDatTextWhenAbove5 = 0;

	[Space(10f)]
	[Indent]
	[VisibleIf(nameof(GimmeDatProperty), invertVisibility: false)]
	public string hereIsYoTextBoi = "Here's yo text boi!";

	public bool GimmeDatProperty => gimmeDatTextWhenAbove5 > 5 ? true : false;


	public bool GimmeDat()
	{
		return gimmeDatTextWhenAbove5 > 5 ? true : false;
	}
}