using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	private static DialogueManager _instance;
	public static DialogueManager GetInstance()
	{
		if (_instance == null) {
			_instance = FindObjectOfType<DialogueManager>();
		}
		return _instance;
	}

	public class ChoiceOption
	{
		public string text;
		public Action onChoose;

		public ChoiceOption(string text, Action onChoose)
		{
			this.text = text;
			this.onChoose = onChoose;
		}
	}

	public TMP_Text textUi;
	public GameObject textBox;

	private Action _currentCloseAction = null;
	private ChoiceOption[] _currentOptions = null;
	private int _currentChoiceSelection;

	private void Start()
	{
		textBox.SetActive(false);
	}

	public void ShowDialogue(Speaker speaker, string text, Action onClose)
	{
		if (IsCurrentlyActive) return;
		_currentCloseAction = onClose;
		textUi.text = speaker.GetDisplayName() + ": " + text;
		textBox.SetActive(true);
	}

	public void ShowChoice(ChoiceOption[] options)
	{
		if (IsCurrentlyActive) return;
		if (options.Length > 4) Debug.LogError("Cannot really show this many options!");
		_currentOptions = options;
		_currentChoiceSelection = 0;
		RefreshOptions();
		textBox.SetActive(true);
		// TODO actually let the user decide!
		options.Last().onChoose();
	}

	public void Continue()
	{
		if (_currentCloseAction != null) {
			var action = _currentCloseAction;
			_currentCloseAction = null;
			textBox.SetActive(false);
			action();
		}
		if (_currentOptions != null) {
			var action = _currentOptions[_currentChoiceSelection].onChoose;
			_currentChoiceSelection = 0;
			_currentOptions = null;
			textBox.SetActive(false);
			action();
		}
	}

	public bool IsCurrentlyActive => textBox.activeSelf;

	private void RefreshOptions()
	{
		textUi.text = String.Join("\n", _currentOptions.Select((o, i) =>
			(_currentChoiceSelection == i ? "<mspace=1em>> </mspace>" : "<mspace=1em>  </mspace>") + o.text
			).ToList());
	}

	private void Update()
	{
		if (_currentOptions != null) {
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				_currentChoiceSelection++;
				if (_currentChoiceSelection >= _currentOptions.Length) _currentChoiceSelection = 0;
				RefreshOptions();
			} else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				_currentChoiceSelection--;
				if (_currentChoiceSelection < 0) _currentChoiceSelection = _currentOptions.Length - 1;
				RefreshOptions();
			}
		}
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) ||
		    Input.GetKeyDown(KeyCode.KeypadEnter)) {
			Continue();
		}
	}
}
