using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldHoverInfo : MonoBehaviour, IHoverInfo
{
	[TextArea(1,10)]
	public string Text;

	private void Awake()
	{
		SetText(Text);
	}

	public void SetText(string newText)
	{
		Text = newText;
	}

	public string GetText()
	{
		return Text;
	}
	
	public void OnMouseEnter()
	{
		HoverMaster.GetInstance().ShowInfo(this);
	}

	public void OnMouseExit()
	{
		HoverMaster.GetInstance().HideInfo(this);
	}

	public void OnMouseDown()
	{
		HoverMaster.GetInstance().ForceHide();
	}

	private void OnDisable()
	{
		var hoverMaster = HoverMaster.GetInstance();
		if (hoverMaster != null) {
			hoverMaster.HideInfo(this);
		}
	}

	public Transform GetTransform()
	{
		return transform;
	}
}
