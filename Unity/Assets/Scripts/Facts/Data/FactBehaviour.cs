using System.Collections.Generic;
using UnityEngine;

public class FactBehaviour : MonoBehaviour
{
	public string Text; 
	public string InternalID => name;
	public string ID => $"{Topic.InternalID}_{InternalID}";

	private TopicBehaviour _topic;
	public TopicBehaviour Topic {
		get {
			if (_topic == null) _topic = GetComponentInParent<TopicBehaviour>();
			return _topic;
		}
	}

	public List<FactReference> Dependencies;

	[ExecuteInEditMode]
	private void Awake()
	{
	}
}
