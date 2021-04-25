using System.Collections.Generic;
using UnityEngine;

public class FactBehaviour : MonoBehaviour
{
	[TextArea(3, 1000)]
	public string Text;
	public string InternalID => name;
	public string ID => $"{Topic.InternalID}_{InternalID}";

	private TopicBehaviour _topic;
	public TopicBehaviour Topic => _topic ??= GetComponentInParent<TopicBehaviour>();

	public List<FactReference> Dependencies;

	public FactState State {
		get => FactsManager.GetFactState(ID);
		set => FactsManager.SetFactState(ID, value);
	}
	
	public bool Discovered => State == FactState.Discovered;
	
	public void Discover()
	{
		FactsManager.Discover(ID);
	}

}
