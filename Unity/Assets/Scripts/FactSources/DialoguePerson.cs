using System;
using System.Linq;
using UnityEngine;

public class DialoguePerson : MonoBehaviour
{
	public void SpeakTo()
	{
		var options = GetComponents<DialogueOption>().Where(o => o.IsAvailable()).ToList();
		if (options.Count == 0) return;
		// TODO UI based selection of options, don't just execute any!
		options.Last().Execute();
	}

	private void OnMouseDown()
	{
		SpeakTo();
	}
}
