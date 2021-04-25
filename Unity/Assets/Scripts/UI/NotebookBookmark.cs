using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotebookBookmark : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	private static readonly Color HighlightColor = Color.white;
	private static readonly Color NonHighlightColor = new Color(0.7f, 0.7f, 0.7f);
	
	public UnityEvent OnClick;
	private Image _image;

	private void Start()
	{
		_image = GetComponent<Image>();
		OnPointerExit(null);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_image.color = HighlightColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_image.color = NonHighlightColor;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnPointerExit(null);
		OnClick?.Invoke();
	}
}
