using UnityEngine;
using UnityEngine.Events;

public class ClickUtil : MonoBehaviour
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