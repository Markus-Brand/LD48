using System.Collections.Generic;
using UnityEngine;

public class Document : MonoBehaviour
{
	[TextArea(3, 999)]
	public string text;
	public List<FactOccurrence> facts;

	public void OpenText()
	{
		Debug.Log("You are now reading: " + text);
		foreach (var factOccurrence in facts) {
			factOccurrence.fact.Discover();
		}
	}

	private void OnMouseDown()
	{
		OpenText();
	}
}
