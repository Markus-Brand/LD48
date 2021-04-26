using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueElement
{
	public TopicReference Speaker;
	public string Text;
	public List<FactReference> FactsToLearn;

	public void Execute(List<DialogueElement> restDialogueElements, int nextIndex)
	{
		DialogueManager.GetInstance().ShowDialogue(Speaker.Topic, Text, () => {
			foreach (var fact in FactsToLearn) {
				fact.Discover();
			}
			if (nextIndex < restDialogueElements.Count) {
				restDialogueElements[nextIndex].Execute(restDialogueElements, nextIndex + 1);
			}
		});
	}
}
