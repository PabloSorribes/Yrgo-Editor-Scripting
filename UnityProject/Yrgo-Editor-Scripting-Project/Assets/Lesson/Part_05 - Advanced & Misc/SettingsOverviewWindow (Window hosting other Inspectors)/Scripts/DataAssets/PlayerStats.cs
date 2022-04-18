using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettingsOverviewWindowExample.Data
{
	[CreateAssetMenu(fileName = "PlayerStats", menuName = "YRGO/Part 05/SettingsOverviewWindow/PlayerStats", order = 1)]
	public class PlayerStats : ScriptableObject
	{
		public int myPlayerInt = 5;
	}
}
