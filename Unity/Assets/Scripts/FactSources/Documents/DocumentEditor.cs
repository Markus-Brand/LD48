
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Document), true)]
public class DocumentEditor : Editor
{

	private SerializedProperty _text;
	private bool _repaintNext;
	
	void OnEnable()
	{
		_text = serializedObject.FindProperty("Text");
	}
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (EditorGUILayout.DropdownButton(new GUIContent("Add Fact"), FocusType.Passive)) {
			FactReferenceEditor.ShowFactsMenuFor(null, s => {
				_text.stringValue += $"<fact={s}>TEXT</fact>";
				serializedObject.ApplyModifiedProperties();
				_repaintNext = true;
			}, false);
		}
		
		if (EditorGUILayout.DropdownButton(new GUIContent("Add Language"), FocusType.Passive)) {
			var menu = new GenericMenu();
			menu.AddItem(new GUIContent("Precursor"), false, () => {
				_text.stringValue += $"<lang=Precursor>TEXT</lang>";
				serializedObject.ApplyModifiedProperties();
				_repaintNext = true;
			});
			menu.AddItem(new GUIContent("Kyr"), false, () => {
				_text.stringValue += $"<lang=Kyr>TEXT</lang>";
				serializedObject.ApplyModifiedProperties();
				_repaintNext = true;
			});
			menu.ShowAsContext();
		}
	}

	public override bool RequiresConstantRepaint()
	{
		if (!_repaintNext) return false;
		_repaintNext = false;
		return true;
	}
}

#endif
