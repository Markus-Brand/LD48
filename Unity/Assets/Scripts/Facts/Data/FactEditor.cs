
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FactBehaviour), true)]
public class FactEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var fact = target as FactBehaviour;
		if (GUILayout.Button("Add Sibling Fact")) {
			TopicBehaviour.CreateFactFor(fact.Topic);
		}
		if (GUILayout.Button("Copy ID")) {
			CopyToClipboard(fact.ID);
		}
	}
	
	public static void CopyToClipboard(string s)
	{
		var te = new TextEditor {text = s};
		te.SelectAll();
		te.Copy();
	}
	
}
