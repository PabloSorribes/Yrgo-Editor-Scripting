using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettingsOverviewWindowExample.Data
{
	[CreateAssetMenu(fileName = "OtherStats", menuName = "YRGO/Part 05/SettingsOverviewWindow/OtherStats", order = 1)]
	public class OtherStats : ScriptableObject
	{
		public int myOtherStatsModifierInt = 0;
	}
}