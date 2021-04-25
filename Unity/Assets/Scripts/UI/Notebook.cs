using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WgEventSystem;
using WgEventSystem.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Notebook : MonoBehaviour
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
	public float NotificationScale = 1.2f;
	public GameObject ClosedNotebook;
	public GameObject OpenNotebook;
	public TMP_Text NotesText;

	private readonly SmoothToggle _open = new SmoothToggle(false, 0.2f);
	private readonly AutoResettingSmoothToggle _notificationBlink = new AutoResettingSmoothToggle(false, 0.2f);

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
		EventManager.getInstance().On<FactStateChangedEvent>(e => UpdateText());
		EventManager.getInstance().On<FactStateChangedEvent>(e => NotifyBlink());
		UpdateText();
	}

	private void Update()
	{
		_open.Update();
		_notificationBlink.Update();
		ClosedNotebook.transform.localPosition = _open.Lerp(_initialPosition, _openPosition);
		ClosedNotebook.transform.localScale = _open.Lerp(_initialScale, _openScale) * _notificationBlink.Lerp(1, NotificationScale);
		var openVisible = _open.IsTrue();
		OpenNotebook.SetActive(openVisible);
		ClosedNotebook.SetActive(!openVisible);

		if (Input.GetKeyDown(KeyCode.Tab)) {
			_open.Flip();
		}

		if (Input.GetKey(KeyCode.H)) {
			StartCoroutine(LoadScene("Home Room"));
		} else if (Input.GetKey(KeyCode.B)) {
			StartCoroutine(LoadScene("SampleScene"));
		} else if (Input.GetKey(KeyCode.T)) {
			StartCoroutine(LoadScene("Throne Room"));
		} else if (Input.GetKey(KeyCode.R)) {
			StartCoroutine(LoadScene("Ruins"));
		}
	}

	private static IEnumerator LoadScene(string name)
	{
		var load = SceneManager.LoadSceneAsync(name);
		while (!load.isDone) {
			yield return null;
		}
	}

	public void OpenClose()
	{
		_open.Flip();
	}

	public void NotifyBlink()
	{
		_notificationBlink.SetTrue();
	}

	private void UpdateText()
	{
		NotesText.text = GetNotebookFactText();
	}
}
