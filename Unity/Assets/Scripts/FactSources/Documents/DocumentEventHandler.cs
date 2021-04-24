using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DocumentEventHandler : MonoBehaviour, IPointerClickHandler
{

	public UnityEvent<string> ClickedLink;
	
	private RectTransform _transform;
	private Camera _camera;
	private TMP_Text _textComponent;
	
	private void Start()
	{
		ClickedLink ??= new UnityEvent<string>();
		_camera = Camera.main;
		_transform = GetComponent<RectTransform>();
		_textComponent = GetComponent<TMP_Text>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textComponent, Input.mousePosition, _camera);
		if (linkIndex != -1) {
			var link = _textComponent.textInfo.linkInfo[linkIndex];
			ClickedLink.Invoke(link.GetLinkID());
		}
	}
}
