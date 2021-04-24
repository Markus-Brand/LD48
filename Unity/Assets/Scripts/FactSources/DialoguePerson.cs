using System;
using System.Linq;
using UnityEngine;

public class DialoguePerson : MonoBehaviour
{
	public void SpeakTo()
	{
		var options = GetComponents<DialogueOption>().Where(o => o.IsAvailable()).ToList();
		if (options.Count == 0) return;
		DialogueManager.GetInstance().ShowChoice(
			options.Select(o => new DialogueManager.ChoiceOption(o.displayName, o.Execute)).ToArray()
			);
	}

	private void OnMouseDown()
	{
		SpeakTo();
	}
}
