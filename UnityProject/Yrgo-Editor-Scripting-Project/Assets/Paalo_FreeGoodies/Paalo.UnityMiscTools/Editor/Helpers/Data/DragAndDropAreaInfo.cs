using UnityEngine;

namespace Paalo.UnityMiscTools.EditorTools
{
	/// <summary>
	/// Use this class to create some values for the DragAndDrop-area that you want to create.
	/// </summary>
	public class DragAndDropAreaInfo
	{
		public string DragAreaText
		{
			get => $"Drag {draggedObjectTypeName} or a folder containing some {draggedObjectTypeName} here!";
			//private set => DragAreaText = value;
		}

		public string draggedObjectTypeName = "AudioClips";
		public float dragAreaWidth = 0f;
		public float dragAreaHeight = 35f;

		public Color outlineColor = Color.black;
		public Color backgroundColor = Color.yellow;

		public DragAndDropAreaInfo(string draggedObjectTypeName)
		{
			this.draggedObjectTypeName = draggedObjectTypeName;
		}

		public DragAndDropAreaInfo(string draggedObjectTypeName, Color outlineColor, Color backgroundColor, float dragAreaWidth = 0f, float dragAreaHeight = 35f)
		{
			this.draggedObjectTypeName = draggedObjectTypeName;
			this.outlineColor = outlineColor;
			this.backgroundColor = backgroundColor;
			this.dragAreaWidth = dragAreaWidth;
			this.dragAreaHeight = dragAreaHeight;
		}
	}
}
