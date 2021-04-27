using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public class DialogueOption : MonoBehaviour
{
	private static readonly HashSet<string> DialogueDoneSet = new HashSet<string>();
	
	[FormerlySerializedAs("displayName")] public string DisplayName;
	[FormerlySerializedAs("conditions")] public List<FactReference> Conditions;
	[FormerlySerializedAs("dialogue")] public List<DialogueElement> Dialogue;
	[FormerlySerializedAs("repeatedDialogue")] public List<DialogueElement> RepeatedDialogue;

	public FactReference PreventedBy;
	public UnityEvent DialogueDone;
	
	public bool IsAvailable()
	{
		return Conditions.All(c => c.Discovered) && (PreventedBy.IsNull || !PreventedBy.Discovered);
	}

	public bool HasBeenDoneAlready => DialogueDoneSet.Contains(DisplayName);

	public void Execute()
	{
		if (HasBeenDoneAlready && RepeatedDialogue.Count > 0) {
			RepeatedDialogue[0].Execute(RepeatedDialogue, 1, DialogueDone);
		} else if (Dialogue.Count != 0) {
			Dialogue[0].Execute(Dialogue, 1, DialogueDone);
			DialogueDoneSet.Add(DisplayName);
		}
	}
}
