using UnityEngine;

public class ScriptUsingVarWithBasicManualPropertyDrawer : MonoBehaviour
{
	public int normalInt1;
	[Space]
	public BasicManualSerializedClass myManualBasicSerializedClass;
	[Space]
	public int normalInt2;

	private void Start()
	{
		var myBool = myManualBasicSerializedClass.manualDrawerBoolVariable;
		var myFloat = myManualBasicSerializedClass.manualDrawerFloatVariable;
	}
}
