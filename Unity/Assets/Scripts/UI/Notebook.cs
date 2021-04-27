using System.Collections.Generic;
using System.Linq;
using WgEventSystem;
using WgEventSystem.Events;
using TMPro;
using UnityEngine;

public class Notebook : MonoBehaviour
{
	private static Notebook _instance;

	public static Notebook GetInstance()
	{
		if (_instance == null) {
			_instance = FindObjectOfType<Notebook>();
		}
		return _instance;
	}

	private void Awake()
	{
		if (this != GetInstance()) {
			Destroy(this);
			Destroy(gameObject);
		}
	}

	public static List<string> GetNotebookFactText()
	{
		var result = new List<string>();

		foreach (var topic in FactManager.Instance.Topics) {
			var discoveredFactsOfTopic = GetDiscoveredFactsOfTopic(topic);
			if (discoveredFactsOfTopic.Count == 0) continue;
			var topicText = "<size=130%><align=center>" + topic.CurrentName + "</align></size>\n\n";
			foreach (var fact in discoveredFactsOfTopic) {
				topicText += "- " + fact.Text + "\n";
			}
			topicText += "\n";
			result.Add(topicText);
		}

		return result;
	}

	public static string GetNotebookTocText()
	{
		var result = "<size=200%><align=center>Table of Contents</align></size>\n\n";

		foreach (var topic in FactManager.Instance.Topics) {
			var discoveredFactsOfTopic = GetDiscoveredFactsOfTopic(topic);
			if (discoveredFactsOfTopic.Count == 0) continue;
			result += "> <link=topic:" + topic.InternalID + ">" + topic.CurrentName + "</link>\n";
		}

		return result;
	}

	private static List<Fact> GetDiscoveredFactsOfTopic(Topic topic)
	{
		return topic.Facts.Where(fact => fact.IsDiscovered).ToList();
	}

	public Vector3 OpenDisplacement = Vector3.zero;
	public float OpenScale = 1;
	public float NotificationScale = 1.2f;
	public GameObject ClosedNotebook;
	public GameObject OpenNotebook;
	public TMP_Text NotesTextLeft;
	public TMP_Text NotesTextRight;

	public GameObject NotesContent;
	public GameObject MapContent;
	public GameObject NotesBookmark;
	public GameObject MapBookmark;
	public GameObject PreviousPageButton;
	public GameObject NextPageButton;

	private readonly SmoothToggle _open = new SmoothToggle(false, 0.2f, SmoothToggle.Smoothing.SmoothStep);
	private readonly SmoothToggle _raisedForOpen = new SmoothToggle(false, 0.3f, SmoothToggle.Smoothing.ToTrue);
	private readonly AutoResettingSmoothToggle _notificationBlink = new AutoResettingSmoothToggle(false, 0.2f);

	private Vector3 _initialPosition;
	private Vector3 _initialScale;
	private Vector3 _openPosition;
	private Vector3 _openScale;

	private int _notesPage = 0;
	private List<string> TopicTexts = new List<string>();
	private string TocText = "";
	private Dictionary<string, int> TopicIdToNotesPage;

	private void Start()
	{
		_initialPosition = ClosedNotebook.transform.localPosition;
		_initialScale = ClosedNotebook.transform.localScale;
		_openPosition = _initialPosition + OpenDisplacement;
		_openScale = _initialScale * OpenScale;
		EventManager.getInstance().On<FactStateChangedEvent>(e => UpdateText());
		EventManager.getInstance().On<FactStateChangedEvent>(e => NotifyBlink());
		EventManager.getInstance().On<FactStateChangedEvent>(e => SwitchToNotes());
		UpdateText();
		SwitchToNotes();
	}

	private void Update()
	{
		_open.Update();
		_notificationBlink.Update();
		_raisedForOpen.Update();
		
		ClosedNotebook.transform.localPosition = _open.Lerp(_initialPosition, _openPosition) + _raisedForOpen.CurrentValue * 100 * Vector3.up;
		ClosedNotebook.transform.localScale = _open.Lerp(_initialScale, _openScale) * _notificationBlink.Lerp(1, NotificationScale);
		var openVisible = _open.IsTrue();
		OpenNotebook.SetActive(openVisible);
		ClosedNotebook.SetActive(!openVisible);

		if (Input.GetKeyDown(KeyCode.Tab)) {
			_open.Flip();
		}

		if (Input.GetKeyDown(KeyCode.M)) {
			if (_open.IsTrue() && MapContent.activeSelf) {
				Close();
			} else {
				OpenMap();
			}
		}
		if (Input.GetKeyDown(KeyCode.N)) {
			if (_open.IsTrue() && NotesContent.activeSelf) {
				Close();
			} else {
				OpenNotes();
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			_open.SetFalse();
		}
		if (_open.IsTrue()) {
			_raisedForOpen.SetFalse(true);
		}
	}
	
	public void OpenClose()
	{
		_open.Flip();
	}

	public void OpenMap()
	{
		SwitchToMap();
		_open.SetTrue();
	}

	public void OpenNotes()
	{
		SwitchToNotes();
		_open.SetTrue();
	}

	public void SwitchToMap()
	{
		_notesPage = 0;
		NotesContent.SetActive(false);
		MapContent.SetActive(true);
		MapBookmark.SetActive(false);
		NotesBookmark.SetActive(true);
		PreviousPageButton.SetActive(false);
		NextPageButton.SetActive(true);
	}

	public void SwitchToNotes()
	{
		_notesPage = 0;
		UpdateText();
		MapContent.SetActive(false);
		NotesContent.SetActive(true);
		NotesBookmark.SetActive(false);
		MapBookmark.SetActive(true);
		PreviousPageButton.SetActive(true);
		NextPageButton.SetActive(TopicTexts.Count >= 2);
	}

	public void Close()
	{
		_raisedForOpen.SetFalse();
		_open.SetFalse();
	}

	public void NotifyBlink()
	{
		_notificationBlink.SetTrue();
	}

	public void GoToPreviousPage()
	{
		if (NotesContent.activeSelf) {
			if (_notesPage == 0) {
				SwitchToMap();
			} else {
				_notesPage--;
				UpdateText();
				PreviousPageButton.SetActive(true);
				NextPageButton.SetActive(true);
			}
		}
	}

	public void GoToNextPage()
	{
		if (MapContent.activeSelf) {
			SwitchToNotes();
		} else if (_notesPage < TopicTexts.Count / 2 + 1) {
			_notesPage++;
			UpdateText();
			PreviousPageButton.SetActive(true);
			NextPageButton.SetActive(TopicTexts.Count > _notesPage * 2 + 1);
		}
	}

	public void OnClosedBookHover(bool hover)
	{
		_raisedForOpen.SetTo(hover);
	}

	private void UpdateText()
	{
		TopicTexts = GetNotebookFactText();
		TocText = GetNotebookTocText();
		var availableTopics = FactManager.Instance.Topics.Where(t => GetDiscoveredFactsOfTopic(t).Count > 0).ToList();
		TopicIdToNotesPage = availableTopics.ToDictionary(t => t.InternalID, t => (availableTopics.IndexOf(t) + 1) / 2);
		
		if (_notesPage == 0) {
			NotesTextLeft.text = TocText;
		} else if (TopicTexts.Count > 1) {
			NotesTextLeft.text = TopicTexts[_notesPage * 2 - 1];
		} else {
			NotesTextLeft.text = "";
		}
		if (TopicTexts.Count > _notesPage * 2) {
			NotesTextRight.text = TopicTexts[_notesPage * 2];
		} else {
			NotesTextRight.text = "";
		}
	}

	public bool IsOpen()
	{
		return _open.IsTrueBy(0.1f);
	}

	public void PerformLinkAction(string action)
	{
		if (action.StartsWith("topic:")) {
			var targetTopicId = action.Substring("topic:".Length);
			_notesPage = TopicIdToNotesPage[targetTopicId];
			UpdateText();
		} else {
			Debug.LogError("Cannot handle link action: " + action);
		}
	}
}
