using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Topic), true)]
public class TopicEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var topic = target as Topic;
		if (GUILayout.Button("Add Fact")) {
			Topic.CreateFactFor(topic);
		}
	}
}
