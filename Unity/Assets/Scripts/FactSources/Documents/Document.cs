using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(HoverTint))]
public class Document : MonoBehaviour, IHoverInfo
{
	public GameObject Overlay;
	public UnityEvent OnDocumentOpen;

	public string Title;

	[TextArea(3, 999)]
	public string Text;

	
	public void OpenText()
	{
		Debug.Log("You are now reading: " + Text);
		var overlayObject = Instantiate(Overlay);
		var overlay = overlayObject.GetComponent<DocumentOverlay>();
		overlay.Text = Text;
		OnDocumentOpen.Invoke();
	}

	private void OnMouseDown()
	{
		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) OpenText();
	}
	
	public void OnMouseEnter()
	{
		HoverMaster.GetInstance().ShowInfo(this);
	}

	public void OnMouseExit()
	{
		HoverMaster.GetInstance().HideInfo(this);
	}

	public Transform GetTransform()
	{
		return transform;
	}

	public string GetHoverText()
	{
		return Title + "\nMaybe there are some facts to learn?";
	}
}
