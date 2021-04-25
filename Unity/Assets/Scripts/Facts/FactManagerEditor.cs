

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FactManager), true)]
public class FactManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var manager = target as FactManager;
		if (GUILayout.Button("Add Topic")) {
			FactManager.CreateTopicFor(manager);
		}
	}
}
