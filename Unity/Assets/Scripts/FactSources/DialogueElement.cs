using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueElement
{
	public Speaker speaker;
	public string text;
	public List<FactReference> factsToLearn;

	public void Execute(List<DialogueElement> restDialogueElements, int nextIndex)
	{
		DialogueManager.GetInstance().ShowDialogue(speaker, text, () => {
			foreach (var fact in factsToLearn) {
				fact.Discover();
			}
			if (nextIndex < restDialogueElements.Count) {
				restDialogueElements[nextIndex].Execute(restDialogueElements, nextIndex + 1);
			}
		});
	}
}
