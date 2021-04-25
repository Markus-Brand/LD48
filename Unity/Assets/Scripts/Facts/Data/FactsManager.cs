using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FactsManager : MonoBehaviour
{
	
	public static FactsManager Instance { get; private set; }
	public List<TopicBehaviour> Topics => GetComponentsInChildren<TopicBehaviour>().ToList();

	private void Awake()
	{
		Instance ??= this;
	}

	[MenuItem("GameObject/Add Topic", true, 0)]
	static bool createTopicVisible(MenuCommand menuCommand)
	{
		return Selection.activeGameObject.GetComponent<FactsManager>() != null;
	}
	
	[MenuItem("GameObject/Add Topic", false, 0)]
	static void createTopic(MenuCommand menuCommand)
	{
		GameObject go = new GameObject("UNNAMED");
		go.AddComponent<TopicBehaviour>();
		// Ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
		Undo.RegisterCreatedObjectUndo(go, $"Create {go.name} topic");
		Selection.activeObject = go;
	}
}
