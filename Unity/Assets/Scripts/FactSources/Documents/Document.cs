using System;
using System.Collections.Generic;
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
		if (HoverMaster.GetInstance().FullscreenUiOpen) return;
		CursorManager.GetInstance().SetInvestigateCursor();
		HoverMaster.GetInstance().ShowInfo(this);
	}

	public void OnMouseExit()
	{
		CursorManager.GetInstance().SetDefaultCursor();
		HoverMaster.GetInstance().HideInfo(this);
	}

	public Transform GetTransform()
	{
		return transform;
	}

	public string GetHoverText()
	{
		return Title;
	}
	
#if UNITY_EDITOR
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.D)) {
			foreach (var factId in GetContainedFactIds()) {
				FactManager.Discover(factId);
			}
		}
	}
#endif

	public List<string> GetContainedFactIds()
	{
		return TextUtilities.GetFactIdsInText(Text);
	}
}
