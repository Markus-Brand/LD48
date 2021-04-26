using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Topic : MonoBehaviour
{
	[Serializable]
	public class NameOption
	{
		public FactReference Condition;
		public string Name;

		public bool Applicable => Condition.State == FactState.Discovered;
	}

	public bool CanBeSpeaker = true;
	public string Name;
	
	public string InternalID => name;

	private List<Fact> _facts;
	public List<Fact> Facts => _facts ??= GetComponentsInChildren<Fact>().ToList();
	
	public List<NameOption> Names;
	
	public string CurrentName => Names.Where(option => option.Applicable).Select(option => option.Name).LastOrDefault() ?? Name;

	private void Awake()
	{
		Names ??= new List<NameOption>();
	}

#if UNITY_EDITOR
	[MenuItem("GameObject/Add Fact", true, 0)]
	static bool createFactVisible(MenuCommand menuCommand)
	{
		return Selection.activeGameObject?.GetComponent<Topic>() != null;
	}

	[MenuItem("GameObject/Add Fact", false, 0)]
	static void createFact(MenuCommand menuCommand)
	{
		CreateFactFor((menuCommand.context as GameObject)?.GetComponent<Topic>());
	}

	public static void CreateFactFor(Topic topic)
	{
		GameObject go = new GameObject("UNNAMED");
		go.AddComponent<Fact>();
		// Ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, topic.gameObject);
		Undo.RegisterCreatedObjectUndo(go, $"Create {go.name} fact");
		Selection.activeObject = go;
	}
#endif

}
