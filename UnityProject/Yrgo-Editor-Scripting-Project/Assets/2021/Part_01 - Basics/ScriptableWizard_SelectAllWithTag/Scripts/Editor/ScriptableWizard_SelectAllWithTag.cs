using UnityEditor;
using UnityEngine;

public class ScriptableWizard_SelectAllWithTag : ScriptableWizard
{
	public string searchTag = "Your tag here";

	[MenuItem("YRGO/Part 01/ScriptableWizard - Select All With Tag...")]
	static void SelectAllWithTagWizard()
	{
		ScriptableWizard.DisplayWizard<ScriptableWizard_SelectAllWithTag>("Select All With Tag...", "Make Selection", "Some Other Button");
	}

	void OnWizardCreate()
	{
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(searchTag);
		Selection.objects = gameObjects;
	}

	private void OnWizardOtherButton()
	{
		Debug.Log($"Clicked other button!");
	}

	private void OnWizardUpdate()
	{
		//Debug.Log($"A value on the Wizard was updated.");
	}
}