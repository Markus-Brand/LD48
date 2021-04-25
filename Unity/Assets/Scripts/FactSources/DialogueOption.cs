using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
	public string displayName;
	public List<FactReference> conditions;
	public List<DialogueElement> dialogue;

	public bool IsAvailable()
	{
		return conditions.All(c => c.Discovered);
	}

	public void Execute()
	{
		if (dialogue.Count == 0) return;
		dialogue[0].Execute(dialogue, 1);
	}
}
