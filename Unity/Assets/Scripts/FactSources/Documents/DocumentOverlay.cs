using System;
using TMPro;
using UnityEngine;

public class DocumentOverlay : MonoBehaviour
{
	public TMP_Text MainText;
	public TMP_Text LastText;

	private string _internalText;
	
	public string Text {
		set {
			_internalText = value;
			UpdateText();
		}
	}

	public void Dismiss()
	{
		Destroy(gameObject);
	}

	public void PerformLinkAction(string action)
	{
		Debug.Log($"Perform Link Action: {action}");
		var split = action.Split(":".ToCharArray());
        var type = split[0];
        switch (type) {
	        case "unlock-fact":
		        FactsManager.Discover(split[1]);
		        UpdateText();
		        break;
        }
	}

	private void UpdateText()
	{
		MainText.text = TextUtilities.TransformText(_internalText);
	}
}
