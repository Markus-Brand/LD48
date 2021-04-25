using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UiLineClickHandler : MonoBehaviour, IPointerClickHandler
{
	public TMP_Text Text;
	public UnityEvent<int> OnClick;
	public UnityEvent<int> OnHover;

	private Camera _camera;
	private int _lastLine = -1;

	private void Start()
	{
		_camera = Camera.main;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke(TMP_TextUtilities.FindNearestLine(Text, Input.mousePosition, _camera));
	}

	private void Update()
	{
		var hoveredLine = TMP_TextUtilities.FindNearestLine(Text, Input.mousePosition, _camera);
		if (hoveredLine != _lastLine) {
			_lastLine = hoveredLine;
			OnHover?.Invoke(hoveredLine);
		}
	}
}
