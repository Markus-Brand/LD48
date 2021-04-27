using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

	public TMP_Text TextUi;
	public TMP_Text SpeakerTextUi;
	public GameObject TextBox;
	public GameObject SpeakerNameBox;

	public Sprite BoxWithName;
	public Sprite BoxWithoutName;

	private Action _currentCloseAction = null;
	private ChoiceOption[] _currentOptions = null;
	private int _currentChoiceSelection;

	[NonSerialized] public bool ShowsHelpText;

	private void Start()
	{
		TextBox.SetActive(false);
	}

	public void ShowDialogue(Topic speaker, string text, Action onClose)
	{
		if (IsCurrentlyActive) return;
		_currentCloseAction = onClose;
		SpeakerTextUi.text = speaker.CurrentName;
		TextUi.text = text;
		SpeakerNameBox.SetActive(true);
		TextBox.GetComponent<Image>().sprite = BoxWithName;
		TextBox.SetActive(true);
	}

	public void ShowChoice(ChoiceOption[] options)
	{
		if (IsCurrentlyActive) return;
		if (options.Length > 4) Debug.LogError("Cannot really show this many options!");
		_currentOptions = options;
		_currentChoiceSelection = 0;
		RefreshOptions();
		TextBox.GetComponent<Image>().sprite = BoxWithoutName;
		TextBox.SetActive(true);
		SpeakerNameBox.SetActive(false);
	}

	public void Continue()
	{
		Action action = null;
		if (_currentCloseAction != null) {
			action = _currentCloseAction;
			_currentCloseAction = null;
		}
		if (_currentOptions != null) {
			action = _currentOptions[_currentChoiceSelection].onChoose;
			_currentChoiceSelection = 0;
			_currentOptions = null;
		}
		TextBox.SetActive(false);
		ShowsHelpText = false;
		action?.Invoke();
	}

	public void OnLineHover(int line)
	{
		if (_currentOptions != null) {
			if (line < 0 || line >= _currentOptions.Length) return;
			if (_currentChoiceSelection != line) {
				_currentChoiceSelection = line;
				RefreshOptions();
			}
		}
	}

	public bool IsCurrentlyActive => TextBox.activeSelf;

	private void RefreshOptions()
	{
		TextUi.text = String.Join("\n", _currentOptions.Select((o, i) =>
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
		    Input.GetKeyDown(KeyCode.KeypadEnter) || (Input.GetKeyDown(KeyCode.Escape) && _currentOptions == null)) {
			Continue();
		}
	}
}
