using System.Collections.Generic;
using UnityEngine;

public class Document : MonoBehaviour
{
	[TextArea(3, 999)]
	public string Text;

	public GameObject Overlay;

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
