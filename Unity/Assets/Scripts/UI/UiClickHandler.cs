using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class UiClickHandler : MonoBehaviour, IPointerClickHandler
{
	public UnityEvent OnClick;
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (OnClick != null) {
			OnClick.Invoke();
		}
	}
}
