using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(HoverTint))]
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
		if (HoverMaster.GetInstance().FullscreenUiOpen) return;
		SpeakTo();
	}
	
	public void OnMouseEnter()
	{
		if (HoverMaster.GetInstance().FullscreenUiOpen) return;
		HoverMaster.GetInstance().ShowInfo(this);
		if (HasOptions()) {
			CursorManager.GetInstance().SetTalkCursor();
		}
	}

	public bool HasOptions()
	{
		var options = GetComponents<DialogueOption>().Where(o => o.IsAvailable()).ToList();
		return options.Count > 0;
	}

	public void OnMouseExit()
	{
		CursorManager.GetInstance().SetDefaultCursor();
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
