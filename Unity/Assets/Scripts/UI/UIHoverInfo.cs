using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverInfo : MonoBehaviour, IHoverInfo, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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

	public string GetHoverText()
	{
		return Text;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		HoverMaster.GetInstance().ShowInfo(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		HoverMaster.GetInstance().HideInfo(this);
	}

	public void OnPointerClick(PointerEventData eventData)
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
