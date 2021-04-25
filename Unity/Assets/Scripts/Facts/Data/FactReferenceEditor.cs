
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FactReference))]
public class FactReferenceEditor : PropertyDrawer
{

	public void showFactsMenuFor(SerializedProperty property)
	{
		GenericMenu menu = new GenericMenu();
		var factsData = PrefabUtility.LoadPrefabContents("Assets/Fact Data.prefab");
		var manager = factsData.GetComponent<FactsManager>();
		var topics = manager.Topics;

		menu.AddItem(
			new GUIContent("NONE"),
			property.stringValue == "",
			() => {
				property.stringValue = "";
				property.serializedObject.ApplyModifiedProperties();
			});
		
		foreach (var topic in topics) {
			foreach (var fact in topic.Facts) {
				var factID = fact.ID;
				menu.AddItem(
					new GUIContent($"{topic.InternalID}/{fact.InternalID}"),
					property.stringValue == factID,
					() => {
						property.stringValue = factID;
						property.serializedObject.ApplyModifiedProperties();
					});
			}
		}
		
		menu.ShowAsContext();
		PrefabUtility.UnloadPrefabContents(factsData);
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		
		EditorGUI.BeginProperty(position, label, property);
		var id = property.FindPropertyRelative("FactID").stringValue;
		var isSet = id != "";
		if (EditorGUI.DropdownButton(position, new GUIContent(isSet ? id : "NONE"), FocusType.Passive)) {
			showFactsMenuFor(property.FindPropertyRelative("FactID"));
		}
		
		EditorGUI.EndProperty();
	}
}