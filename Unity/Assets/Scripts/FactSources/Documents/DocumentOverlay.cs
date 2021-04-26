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
		var split = action.Split(":".ToCharArray());
        var type = split[0];
        switch (type) {
	        case "unlock-fact":
		        FactManager.Discover(split[1]);
		        UpdateText();
		        break;
	        default:
		        Debug.LogError($"Perform Unknown Link Action: {action}");
		        break;
        }
	}

	private void UpdateText()
	{
		MainText.text = TextUtilities.TransformText(_internalText);
	}
}
