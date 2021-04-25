
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FactReference))]
public class FactReferenceEditor : PropertyDrawer
{

	static public void ShowFactsMenuFor(string currentValue, Action<string> handler, bool includeNone = true)
	{
		GenericMenu menu = new GenericMenu();
		var factsData = PrefabUtility.LoadPrefabContents("Assets/Fact Data.prefab");
		var manager = factsData.GetComponent<FactManager>();
		var topics = manager.Topics;

		if (includeNone) menu.AddItem(
			new GUIContent("NONE"),
			currentValue == "",
			() => handler(""));
		
		foreach (var topic in topics) {
			foreach (var fact in topic.Facts) {
				var factID = fact.ID;
				menu.AddItem(
					new GUIContent($"{topic.InternalID}/{fact.InternalID}"),
					currentValue == factID,
					() => handler(factID));
			}
		}
		
		menu.ShowAsContext();
		PrefabUtility.UnloadPrefabContents(factsData);
	}
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		
		EditorGUI.BeginProperty(position, label, property);
		var propertyID = property.FindPropertyRelative("FactID");
		var id = propertyID.stringValue;
		var isSet = id != "";
		if (EditorGUI.DropdownButton(position, new GUIContent(isSet ? id : "NONE"), FocusType.Passive)) {
			ShowFactsMenuFor(propertyID.stringValue, id => {
				propertyID.stringValue = id;
				propertyID.serializedObject.ApplyModifiedProperties();
			});
		}
		
		EditorGUI.EndProperty();
	}
}