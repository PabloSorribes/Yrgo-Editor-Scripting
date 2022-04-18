using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangeObjectHeight))]
public class ChangeObjectHeightEditor : Editor
{
	ChangeObjectHeight m_Target;

	public override void OnInspectorGUI()
	{
		m_Target = (ChangeObjectHeight)target;

		base.OnInspectorGUI();
		DrawChangeHeightSlider();
	}

	private void DrawChangeHeightSlider()
	{
		Vector3 objectPos = m_Target.transform.position;
		objectPos.y = EditorGUILayout.Slider(label: "Object's Height: ", objectPos.y, m_Target.minHeight, m_Target.maxHeight);

		// Need to check if a change actually happened to avoid Recording an Undo-step each frame in OnInspectorGUI().
		if (objectPos.y != m_Target.transform.position.y)
		{
			Undo.RecordObject(m_Target.transform, "Change Object's Height");
			m_Target.transform.position = objectPos;
		}
	}
}
