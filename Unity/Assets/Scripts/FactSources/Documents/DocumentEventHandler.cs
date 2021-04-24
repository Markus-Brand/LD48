using System;
using UnityEngine;
using TMPro;

public class DocumentEventHandler : MonoBehaviour
{
	private RectTransform _transform;
	private Camera _camera;
	private TMP_Text _textComponent;

	private void Start()
	{
		_camera = Camera.main;
		_transform = GetComponent<RectTransform>();
		_textComponent = GetComponent<TMP_Text>();
	}

	private void Update()
	{
		if (TMP_TextUtilities.IsIntersectingRectTransform(_transform, Input.mousePosition, _camera) && Input.GetMouseButtonUp(0)) {
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textComponent, Input.mousePosition, _camera);
			if (linkIndex != -1) {
				var link = _textComponent.textInfo.linkInfo[linkIndex];
				var split = link.GetLinkID().Split(":".ToCharArray());
				var action = split[0];
				var target = split[1];
				if (action == "unlock-fact") {
					((Fact)Enum.Parse(typeof(Fact), target)).Discover();
				}
			}
		}
	}
}
