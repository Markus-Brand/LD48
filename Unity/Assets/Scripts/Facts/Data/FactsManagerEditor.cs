

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FactsManager), true)]
public class FactsManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var manager = target as FactsManager;
		if (GUILayout.Button("Add Topic")) {
			FactsManager.CreateTopicFor(manager);
		}
	}
}
