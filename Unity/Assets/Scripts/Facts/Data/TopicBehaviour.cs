using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TopicBehaviour : MonoBehaviour
{
	public string Name; 
	public string InternalID => name;

	public List<FactBehaviour> Facts => GetComponentsInChildren<FactBehaviour>().ToList();
	
	public Dictionary<FactReference, string> Names;
	
	
	[MenuItem("GameObject/Add Fact", true, 0)]
	static bool createFactVisible(MenuCommand menuCommand)
	{
		return Selection.activeGameObject.GetComponent<TopicBehaviour>() != null;
	}
	
	[MenuItem("GameObject/Add Fact", false, 0)]
	static void createFact(MenuCommand menuCommand)
	{
		GameObject go = new GameObject("UNNAMED");
		go.AddComponent<FactBehaviour>();
		// Ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
		Undo.RegisterCreatedObjectUndo(go, $"Create {go.name} fact");
		Selection.activeObject = go;
	}
	
}
