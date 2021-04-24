using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueElement
{
	public Speaker speaker;
	public string text;
	public List<Fact> factsToLearn;

	public void Execute(List<DialogueElement> restDialogueElements, int nextIndex)
	{
		// TODO show the dialogue, and set the continue-handler to show the next element of the rest elements
		Debug.Log(speaker.GetDisplayName() + ": " + text);
		foreach (var fact in factsToLearn) {
			fact.Discover();
		}
		if (nextIndex < restDialogueElements.Count) {
			restDialogueElements[nextIndex].Execute(restDialogueElements, nextIndex + 1);
		}
	}
}
