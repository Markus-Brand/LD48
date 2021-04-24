using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace Documents
{
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
					Debug.Log("Clicked Link: " + _textComponent.textInfo.linkInfo[linkIndex].GetLinkID());
				}
			}
		}
	}
}
