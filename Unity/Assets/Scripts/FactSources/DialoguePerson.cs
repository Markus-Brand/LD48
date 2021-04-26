using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DialogueOption))]
[RequireComponent(typeof(Collider2D))]
public class DialoguePerson : MonoBehaviour, IHoverInfo
{
	public TopicReference Person;
	
	public void SpeakTo()
	{
		var options = GetComponents<DialogueOption>().Where(o => o.IsAvailable()).ToList();
		if (options.Count == 0) return;
		DialogueManager.GetInstance().ShowChoice(
			options.Select(o => new DialogueManager.ChoiceOption(o.DisplayName, o.Execute))
				.Concat(new[] {new DialogueManager.ChoiceOption("Nothing", () => { })})
				.ToArray()
		);
	}

	private void OnMouseUp()
	{
		SpeakTo();
	}
	
	public void OnMouseEnter()
	{
		HoverMaster.GetInstance().ShowInfo(this);
	}

	public void OnMouseExit()
	{
		HoverMaster.GetInstance().HideInfo(this);
	}

	public Transform GetTransform()
	{
		return transform;
	}

	public string GetHoverText()
	{
		return Person.Topic.CurrentName;
	}
}
