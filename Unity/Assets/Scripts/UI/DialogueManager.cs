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

	private void Start()
	{
		textBox.SetActive(false);
	}

	public void ShowDialogue(Speaker speaker, string text, Action onClose)
	{
		if (IsCurrentlyActive) return;
		_currentCloseAction = () => {
			textBox.SetActive(false);
			onClose();
		};
		textUi.text = speaker.GetDisplayName() + ": " + text;
		textBox.SetActive(true);
	}

	public void ShowChoice(ChoiceOption[] options)
	{
		if (IsCurrentlyActive) return;
		// TODO actually let the user decide!
		options.Last().onChoose();
	}

	public void Continue()
	{
		if (_currentCloseAction != null) {
			var action = _currentCloseAction;
			_currentCloseAction = null;
			action();
		}
	}

	public bool IsCurrentlyActive => textBox.activeSelf;

	private void OnMouseDown()
	{
		Continue();
	}
}
