
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Fact), true)]
public class FactEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		var fact = target as Fact;
		if (GUILayout.Button("Add Sibling Fact")) {
			Topic.CreateFactFor(fact.Topic);
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
