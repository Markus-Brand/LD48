using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using WgEventSystem;
using WgEventSystem.Events;
using Random = UnityEngine.Random;

public class FactManager : MonoBehaviour
{
	public static FactManager Instance { get; private set; }


	private List<Topic> _topics;
	public List<Topic> Topics => _topics ??= GetComponentsInChildren<Topic>().ToList();

	private Dictionary<string, Topic> _topicsById;
	public Dictionary<string, Topic> TopicsById => _topicsById ??= Topics.ToDictionary(t => t.InternalID);

	private Dictionary<string, Fact> _allFacts;

	public Dictionary<string, Fact> AllFacts =>
		_allFacts ??= Topics.SelectMany(topic => topic.Facts).ToDictionary(fact => fact.ID);

	public IEnumerable<Fact> AllDiscoverableFacts => _allFacts.Values.Where(fact => fact.IsDiscoverable);

	public Dictionary<string, FactState> FactStates { get; private set; }


	private void Awake()
	{
		Instance ??= this;
		FactStates = new Dictionary<string, FactState>();
	}

	[MenuItem("GameObject/Add Topic", true, 0)]
	static bool CreateTopicVisible(MenuCommand menuCommand)
	{
		return Selection.activeGameObject?.GetComponent<FactManager>() != null;
	}

	[MenuItem("GameObject/Add Topic", false, 0)]
	static void CreateTopic(MenuCommand menuCommand)
	{
		CreateTopicFor((menuCommand.context as GameObject)?.GetComponent<FactManager>());
	}

	public static void CreateTopicFor(FactManager manager)
	{
		GameObject go = new GameObject("UNNAMED");
		go.AddComponent<Topic>();
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
		if (force || Instance.AllFacts[id].IsDiscoverable) {
			SetFactState(id, FactState.Discovered);
		}
	}

	public static void Discard(string id)
	{
		SetFactState(id, FactState.Discarded);
	}

#if UNITY_EDITOR
	public void GeneratePuml()
	{
		var validFactIDs = new HashSet<string>();

		string content = "@startuml\ndigraph facts {\n\n";
		foreach (var fact in AllFacts.Values) {
			validFactIDs.Add(fact.ID);
			content += fact.ID + " [label =\"" + fact.ID + "\\n" + fact.Text + "\"]\n";
		}
		content += "\n";
		foreach (var fact in AllFacts.Values) {
			foreach (var cond in fact.Dependencies) {
				content += cond.FactID + " -> " + fact.ID + "\n";
			}
			/*if (fact.Dependencies.Count == 0) {
				content += startNodeName + " -> " + fact.ID + "\n";
			}/**/
		}
		content += "\n";

		var notebook = PrefabUtility.LoadPrefabContents("Assets/Prefabs/Notebook Canvas.prefab");
		var sceneToUnlockFact = notebook.GetComponentsInChildren<MapEntry>(true)
			.ToDictionary(m => m.SceneName, m => m.UnlockCondition.FactID);
		PrefabUtility.UnloadPrefabContents(notebook);
		foreach (var scene in sceneToUnlockFact.Keys) {
			content += scene.Replace(" ", "_") + " [label = \" <Scene> " + scene + "\"]\n";
			if (sceneToUnlockFact[scene] != "") {
				content += sceneToUnlockFact[scene] + " -> " + scene.Replace(" ", "_") + "\n";
			}
		}
		content += "\n";

		foreach (var sceneName in sceneToUnlockFact.Keys) {
			var scenePath = "Assets/Scenes/" + sceneName + ".unity";
			var scene = EditorSceneManager.OpenScene(scenePath);
			foreach (var rootGameObject in scene.GetRootGameObjects()) {
				foreach (var dialogueOption in rootGameObject.GetComponentsInChildren<DialogueOption>()) {
					var dialoguePerson = dialogueOption.GetComponent<DialoguePerson>();
					var personId = dialoguePerson?.Person?.TopicID ?? "_";

					var dialogueID = personId + "_" + dialogueOption.DisplayName.Replace(" ", "_");

					content += sceneName.Replace(" ", "_") + " -> " + dialogueID + "\n";
					content += dialogueID + " [label =\"<Dialogue>" + personId + "\\n" +
					           dialogueOption.DisplayName + "\"]\n";

					var dialogueOptionConditions = dialogueOption.Conditions.Select(f => f.FactID).ToList();
					foreach (var condition in dialogueOptionConditions) {
						content += condition + " -> " + dialogueID + "\n";
					}
					foreach (var dialogueElement in
						dialogueOption.Dialogue.Concat(dialogueOption.RepeatedDialogue)) {
						foreach (var factToLearn in dialogueElement.FactsToLearn) {
							if (!validFactIDs.Contains(factToLearn.FactID)) {
								Debug.LogError("Invalid Fact reference to '" + factToLearn.FactID + "' in " +
								               sceneName + " > " + personId + " > " +
								               dialogueOption.DisplayName + " > " + dialogueElement.Text);
							}
							content += dialogueID + " -> " + factToLearn.FactID + "\n";
						}
					}
				}

				var documents = rootGameObject.GetComponentsInChildren<Document>();
				foreach (var document in documents) {
					var documentId = document.name.Replace(" ", "_");

					content += sceneName.Replace(" ", "_") + " -> " + documentId + "\n";
					content += documentId + " [label =\"<Document>" + document.name + "\"]\n";
					foreach (var factId in document.GetContainedFactIds()) {
						content += documentId + " -> " + factId + "\n";
					}
				}
			}
		}
		AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Fact Data.prefab"));

		content += "\n}\n@enduml";

		var path = "Assets" + Path.DirectorySeparatorChar + "fact-diagram.puml";
		using (var sw = File.CreateText(path)) {
			sw.Write(content);
		}
	}
#endif
}
