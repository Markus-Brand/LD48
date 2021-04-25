using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WgEventSystem;
using WgEventSystem.Events;
using Random = UnityEngine.Random;

public class FactsManager : MonoBehaviour
{

	public static FactsManager Instance { get; private set; }


	private List<TopicBehaviour> _topics;
	public List<TopicBehaviour> Topics => _topics ??= GetComponentsInChildren<TopicBehaviour>().ToList();
	
	private Dictionary<string, FactBehaviour> _allFacts;
	public Dictionary<string, FactBehaviour> AllFacts => _allFacts ??= Topics.SelectMany(topic => topic.Facts).ToDictionary(fact => fact.ID);

	public Dictionary<string, FactState> FactStates { get; private set; }
	
	
	private void Awake()
	{
		Instance ??= this;
		FactStates = new Dictionary<string, FactState>();
	}

	[MenuItem("GameObject/Add Topic", true, 0)]
	static bool CreateTopicVisible(MenuCommand menuCommand)
	{
		return Selection.activeGameObject?.GetComponent<FactsManager>() != null;
	}
	
	[MenuItem("GameObject/Add Topic", false, 0)]
	static void CreateTopic(MenuCommand menuCommand)
	{
		CreateTopicFor((menuCommand.context as GameObject)?.GetComponent<FactsManager>());
	}
	
	public static void CreateTopicFor(FactsManager manager)
	{
		GameObject go = new GameObject("UNNAMED");
		go.AddComponent<TopicBehaviour>();
		// Ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, manager.gameObject);
		Undo.RegisterCreatedObjectUndo(go, $"Create topic");
		Selection.activeObject = go;
	}

	public static FactState GetFactState(string id)
	{
#if UNITY_EDITOR
		if (!Instance.AllFacts.ContainsKey(id)) Debug.LogWarning($"Accessing unknown Fact {id}");
#endif
		return Instance.FactStates.ContainsKey(id) ? Instance.FactStates[id] : FactState.Undiscovered;
	}
	
	public static void SetFactState(string id, FactState state)
	{
#if UNITY_EDITOR
		if (!Instance.AllFacts.ContainsKey(id)) Debug.LogWarning($"Accessing unknown Fact {id}");
#endif
		if (state == GetFactState(id)) return;
		Instance.FactStates[id] = state;
		EventManager.getInstance().Trigger(new FactStateChangedEvent());
	}
	
	public static void Discover(string id, bool force = false)
	{
		if (force || Instance.AllFacts[id].Dependencies.All(reference => reference.Discovered)) {
			SetFactState(id, FactState.Discovered);
		}
	}
	
#if UNITY_EDITOR
	public static void GeneratePuml()
	{
		string content = "@startuml\ndigraph facts {\n\n";
		foreach (var fact in Instance.AllFacts.Values) {
			content += fact.ID + " [label =\"" + fact.ID + "\n" + fact.Text + "\"]\n";
		}
		content += "\n";
		foreach (var fact in Instance.AllFacts.Values) {
			foreach (var cond in fact.Dependencies) {
				content += cond.FactID + " -> " + fact.ID + "\n";
			}
		}
		content += "\n}\n@enduml";

		string path = "Assets" + Path.DirectorySeparatorChar + "fact-diagram.puml";
		using (var sw = File.CreateText(path)) {
			sw.Write(content);
		}
	}
#endif
}
