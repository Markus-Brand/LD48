#if UNITY_EDITOR

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
		if (GUILayout.Button("Generate Topic Graph PUML")) {
			manager.GeneratePuml();
		}
	}
}
#endif
