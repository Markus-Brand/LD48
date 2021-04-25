using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(HoverTint))]
public class Document : MonoBehaviour
{
	public GameObject Overlay;

	[TextArea(3, 999)]
	public string Text;

	public void OpenText()
	{
		Debug.Log("You are now reading: " + Text);
		var overlayObject = Instantiate(Overlay);
		var overlay = overlayObject.GetComponent<DocumentOverlay>();
		overlay.Text = Text;
	}

	private void OnMouseDown()
	{
		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) OpenText();
	}
}
