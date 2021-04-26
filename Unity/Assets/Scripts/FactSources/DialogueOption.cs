using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogueOption : MonoBehaviour
{
	private static readonly HashSet<string> DialogueDoneSet = new HashSet<string>();
	
	[FormerlySerializedAs("displayName")] public string DisplayName;
	[FormerlySerializedAs("conditions")] public List<FactReference> Conditions;
	[FormerlySerializedAs("dialogue")] public List<DialogueElement> Dialogue;
	[FormerlySerializedAs("repeatedDialogue")] public List<DialogueElement> RepeatedDialogue;

	public bool IsAvailable()
	{
		return Conditions.All(c => c.Discovered);
	}

	public bool HasBeenDoneAlready => DialogueDoneSet.Contains(DisplayName);

	public void Execute()
	{
		if (HasBeenDoneAlready) {
			if (RepeatedDialogue.Count == 0) return;
			RepeatedDialogue[0].Execute(RepeatedDialogue, 1);
		} else {
			if (Dialogue.Count == 0) return;
			Dialogue[0].Execute(Dialogue, 1);
			DialogueDoneSet.Add(DisplayName);
		}
	}
}
