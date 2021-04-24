using System;
using UnityEngine;

public class DialoguePerson : MonoBehaviour
{
	public void SpeakTo()
	{
		var options = GetComponents<DialogueOption>();
		
	}

	private void OnMouseDown()
	{
		SpeakTo();
	}
}
