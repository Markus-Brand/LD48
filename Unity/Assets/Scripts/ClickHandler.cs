using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ClickHandler : MonoBehaviour
{
	public UnityEvent OnClick;

	private void OnMouseDown()
	{
		if (OnClick != null) {
			OnClick.Invoke();
		}
	}
}
