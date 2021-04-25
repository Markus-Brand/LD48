using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopicBehaviour), true)]
public class TopicEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var topic = target as TopicBehaviour;
		if (GUILayout.Button("Add Fact")) {
			TopicBehaviour.CreateFactFor(topic);
		}
	}
}
