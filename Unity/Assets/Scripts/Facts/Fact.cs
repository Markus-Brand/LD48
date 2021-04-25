using System.Collections.Generic;
using System.Linq;
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
	
	[TextArea(3, 1000)]
	public string HelpText;

	public FactState State {
		get => FactManager.GetFactState(ID);
		set => FactManager.SetFactState(ID, value);
	}
	
	public bool IsDiscovered => State == FactState.Discovered;
	public bool IsDiscoverable => State == FactState.Undiscovered && Dependencies.All(reference => reference.Discovered);

	public void Discover()
	{
		FactManager.Discover(ID);
	}

}
