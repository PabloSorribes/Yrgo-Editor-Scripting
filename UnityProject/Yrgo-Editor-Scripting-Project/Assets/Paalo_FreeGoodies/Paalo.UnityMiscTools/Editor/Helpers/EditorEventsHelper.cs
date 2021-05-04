using UnityEditor;
using UnityEngine;

namespace Paalo.UnityMiscTools.EditorTools
{
	public static class EditorEventsHelper
	{
		/// <summary>
		/// The special command name strings that Unity uses for eg. copy and paste. 
		/// Use them to validate and execute these special commands in an OnGui()-method.
		/// <para></para>
		/// Source: 'Event.commandName'
		/// <para></para>
		/// See: https://docs.unity3d.com/ScriptReference/Event-commandName.html
		/// </summary>
		public static class CommandNames
		{
			public static readonly string Copy = "Copy";
			public static readonly string Cut = "Cut";
			public static readonly string Paste = "Paste";
			public static readonly string Delete = "Delete";
			public static readonly string SoftDelete = "SoftDelete";
			public static readonly string Duplicate = "Duplicate";
			public static readonly string FrameSelected = "FrameSelected";
			public static readonly string FrameSelectedWithLock = "FrameSelectedWithLock";
			public static readonly string SelectAll = "SelectAll";
			public static readonly string Find = "Find";
			public static readonly string FocusProjectWindow = "FocusProjectWindow";
		}
	}
}
