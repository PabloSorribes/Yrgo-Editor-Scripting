using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SettingsOverviewWindowExample.Runtime
{
	public class PlayerController : MonoBehaviour
	{
		public SettingsOverviewWindowExample.Data.PlayerStats playerStats;

		private int MyCalculatedInt => playerStats.myPlayerInt * 2;

		public void UpdateStatsFromEditorWindow()
		{
			
		}
	}
}