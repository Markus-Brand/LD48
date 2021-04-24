using System;
using System.Linq;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	private static DialogueManager _instance;
	public static DialogueManager GetInstance()
	{
		if (_instance == null) {
			//_instance = FindObjectOfType<DialogueManager>();
			_instance = Camera.main.gameObject.AddComponent<DialogueManager>();
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

	public void ShowDialogue(Speaker speaker, string text, Action onClose)
	{
		// TODO show stuff in ui
		Debug.Log(speaker.GetDisplayName() + ": " + text);
		Invoker.InvokeScaled(() => onClose(), 0.5f);
	}

	public void ShowChoice(ChoiceOption[] options)
	{
		// TODO actually let the user decide!
		options.Last().onChoose();
	}
}
