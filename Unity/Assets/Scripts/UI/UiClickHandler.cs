using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class UiClickHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	public UnityEvent OnClick;
	public UnityEvent<bool> OnHover;
	
	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnHover?.Invoke(true);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OnHover?.Invoke(false);
	}
}
