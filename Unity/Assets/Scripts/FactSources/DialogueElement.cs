using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DialogueElement
{
	public TopicReference Speaker;
	[TextArea(3, 10)]
	public string Text;
	public List<FactReference> FactsToLearn;

	public void Execute(List<DialogueElement> restDialogueElements, int nextIndex, UnityEvent dialogueDone = null)
	{
		DialogueManager.GetInstance().ShowDialogue(Speaker.Topic, Text, () => {
			FactsToLearn.ForEach(fact => fact.Discover());
			if (nextIndex < restDialogueElements.Count) {
				restDialogueElements[nextIndex].Execute(restDialogueElements, nextIndex + 1, dialogueDone);
			} else {
				dialogueDone?.Invoke();
			}
		});
		Invoker.InvokeUnscaled(() => FactsToLearn.ForEach(fact => fact.Discover()), 0.25f);
	}
}
