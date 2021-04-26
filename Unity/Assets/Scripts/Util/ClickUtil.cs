using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class ClickUtil : HoverTint
{
	public UnityEvent Clicked;

	public FactReference FactToLearn;
	
	private void OnMouseUp()
	{
		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
		if (!FactToLearn.IsNull) FactToLearn.Discover();
		Clicked?.Invoke();
	}
}