using System.Collections.Generic;
using UnityEngine;

public class Document : MonoBehaviour
{
	[TextArea(3, 999)]
	public string Text;

	public GameObject Overlay;
	public List<FactOccurrence> Facts;

	public void OpenText()
	{
		Debug.Log("You are now reading: " + Text);
		var overlayObject = Instantiate(Overlay);
		var overlay = overlayObject.GetComponent<DocumentOverlay>();
		overlay.Text = Text;
		/*foreach (var factOccurrence in Facts) {
			factOccurrence.fact.Discover();
		}*/
	}

	private void OnMouseDown()
	{
		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) OpenText();
	}
}
