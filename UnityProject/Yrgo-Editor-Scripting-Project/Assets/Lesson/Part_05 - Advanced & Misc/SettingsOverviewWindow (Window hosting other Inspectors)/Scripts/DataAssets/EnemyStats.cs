using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettingsOverviewWindowExample.Data
{
	[CreateAssetMenu(fileName = "EnemyStats", menuName = "YRGO/Part 05/SettingsOverviewWindow/EnemyStats", order = 1)]
	public class EnemyStats : ScriptableObject
	{
		public int myEnemyInt = 10;
	}
}