using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using WgEventSystem;
using WgEventSystem.Events;

public class NewFactPopup : MonoBehaviour
{
	public TMP_Text DisplayText;

	private readonly SmoothValue _upTranslation = new SmoothValue(0);
	private readonly List<string> _shownFactIds = new List<string>();
	private bool _wantsToClose = false;
	private float _upTime = 0;
	private const float MaxUpTime = 5f;

	private void Start()
	{
		EventManager.getInstance().On<FactStateChangedEvent>(FactStateChanged);
		UpdateTextsAndPosition();
	}

	private void Update()
	{
		_upTranslation.Update();
		if (_shownFactIds.Count > 0) {
			if (_upTranslation.IsStationary && !_upTranslation.CurrentValue.Equals(0)) {
				_upTime += Time.deltaTime;
			}
			if (_upTime >= MaxUpTime && !_wantsToClose) {
				_wantsToClose = true;
				UpdateTextsAndPosition();
			}
		}
		if (_wantsToClose && _upTranslation.CurrentValue.Equals(0)) {
			_shownFactIds.Clear();
			_wantsToClose = false;
			_upTime = 0;
		}
		GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x,
			-_upTranslation.CurrentValue);
		// transform.position = new Vector3(transform.position.x, _upTranslation.CurrentValue, transform.position.z);
	}

	private void FactStateChanged(FactStateChangedEvent e)
	{
		if (e.NewState == FactState.Discovered) {
			_shownFactIds.Add(e.FactId);
			_wantsToClose = false;
			UpdateTextsAndPosition();
		}
	}

	private void UpdateTextsAndPosition()
	{
		var topics = new HashSet<string>();
		foreach (var f in _shownFactIds) {
			topics.Add(FactManager.Instance.AllFacts[f].Topic.CurrentName);
		}
		var topicsSorted = new List<string>(topics);
		topicsSorted.Sort();
		string result = "";
		foreach (var topic in topics) {
			result += topic + "\n";
			foreach (var f in _shownFactIds) {
				if (FactManager.Instance.AllFacts[f].Topic.CurrentName == topic) {
					result += "- " + FactManager.Instance.AllFacts[f].Text + "\n";
				}
			}
		}
		DisplayText.text = result;
		if (_wantsToClose || _shownFactIds.Count == 0) {
			_upTranslation.SetTo(0);
		} else {
			_upTranslation.SetTo(DisplayText.preferredHeight + 150);
		}
	}
}
