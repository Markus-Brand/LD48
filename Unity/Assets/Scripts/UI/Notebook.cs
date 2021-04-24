using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Notebook : MonoBehaviour, IPointerClickHandler
{
	public static string GetNotebookFactText()
	{
		string result = "";

		foreach (var topic in Util.getAllEnumValues<FactTopic>()) {
			var discoveredFactsOfTopic = GetDiscoveredFactsOfTopic(topic);
			if (discoveredFactsOfTopic.Count == 0) continue;
			result += topic.GetDisplayText() + "\n";
			foreach (var fact in discoveredFactsOfTopic) {
				result += "- " + fact.GetDisplayText() + "\n";
			}
			result += "\n";
		}

		return result;
	}

	private static List<Fact> GetDiscoveredFactsOfTopic(FactTopic topic)
	{
		return Util.getAllEnumValues<Fact>()
			.Where(fact => fact.GetState() == FactState.Discovered && fact.GetTopic() == topic).ToList();
	}

	public Vector3 OpenDisplacement = Vector3.zero;
	public float OpenScale = 1;
	public GameObject ClosedNotebook;
	public GameObject OpenNotebook;

	private readonly SmoothToggle _open = new SmoothToggle(false, 0.2f);

	private Vector3 _initialPosition;
	private Vector3 _initialScale;
	private Vector3 _openPosition;
	private Vector3 _openScale;

	private void Start()
	{
		_initialPosition = ClosedNotebook.transform.localPosition;
		_initialScale = ClosedNotebook.transform.localScale;
		_openPosition = _initialPosition + OpenDisplacement;
		_openScale = _initialScale * OpenScale;
	}

	private void Update()
	{
		_open.Update();
		ClosedNotebook.transform.localPosition = _open.Lerp(_initialPosition, _openPosition);
		ClosedNotebook.transform.localScale = _open.Lerp(_initialScale, _openScale);
		var openVisible = _open.IsTrue();
		OpenNotebook.SetActive(openVisible);
		ClosedNotebook.SetActive(!openVisible);

		if (Input.GetKeyDown(KeyCode.Tab)) {
			_open.Flip();
		}
	}
	

	public void OnPointerClick(PointerEventData eventData)
	{
		_open.Flip();
	}
}
