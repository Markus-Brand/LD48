using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Sprite HighlightSprite;
	private Sprite _nonHighlightSprite;
	private Image _image;

	private void Start()
	{
		_image = GetComponent<Image>();
		_nonHighlightSprite = _image.sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_image.sprite = HighlightSprite;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_image.sprite = _nonHighlightSprite;
	}
}
