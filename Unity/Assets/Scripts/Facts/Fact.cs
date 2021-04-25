using System.Collections.Generic;
using UnityEngine;

public class Fact : MonoBehaviour
{
	[TextArea(3, 1000)]
	public string Text;
	public string InternalID => name;
	public string ID => $"{Topic.InternalID}_{InternalID}";

	private Topic _topic;
	public Topic Topic => _topic ??= GetComponentInParent<Topic>();

	public List<FactReference> Dependencies;

	public FactState State {
		get => FactManager.GetFactState(ID);
		set => FactManager.SetFactState(ID, value);
	}
	
	public bool Discovered => State == FactState.Discovered;
	
	public void Discover()
	{
		FactManager.Discover(ID);
	}

}
