using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TopicReference))]
public class TopicReferenceEditor : PropertyDrawer
{
	static public void ShowFactsMenuFor(string currentValue, Action<string> handler, bool includeNone = true)
	{
		GenericMenu menu = new GenericMenu();
		var factsData = PrefabUtility.LoadPrefabContents("Assets/Fact Data.prefab");
		var manager = factsData.GetComponent<FactManager>();
		var topics = manager.Topics;

		if (includeNone)
			menu.AddItem(
				new GUIContent("NONE"),
				currentValue == "",
				() => handler(""));

		foreach (var topic in topics) {
			if (topic.CanBeSpeaker) {
				var topicInternalID = topic.InternalID;
				menu.AddItem(
					new GUIContent($"{topicInternalID}"),
					currentValue == topicInternalID,
					() => handler(topicInternalID));
			}
		}

		menu.ShowAsContext();
		PrefabUtility.UnloadPrefabContents(factsData);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);
		var propertyID = property.FindPropertyRelative("TopicID");
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
