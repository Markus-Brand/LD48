using System;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TextEventHandler : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler
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
			if (link.linkTextfirstCharacterIndex > _textComponent.firstVisibleCharacter) {
				ClickedLink.Invoke(link.GetLinkID());
			}
		}
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textComponent, Input.mousePosition, _camera);
		if (linkIndex != -1) {
			var link = _textComponent.textInfo.linkInfo[linkIndex];
			if (link.linkTextfirstCharacterIndex > _textComponent.firstVisibleCharacter) {
				var split = link.GetLinkID().Split(":".ToCharArray());
				var type = split[0];
				if(type == "unlock-fact" && FactManager.Instance.AllFacts[split[1]].IsDiscoverable) {
					CursorManager.GetInstance().SetInvestigateCursor();
				}
			}
		} else {
			CursorManager.GetInstance().SetDefaultCursor();
		}
	}
}
