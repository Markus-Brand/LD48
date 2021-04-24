using System;
using TMPro;
using UnityEngine;

public class DocumentOverlay : MonoBehaviour
{
	public TMP_Text MainText;
	public TMP_Text LastText;

	public string Text {
		set => MainText.text = value;
	}

	public void Dismiss()
	{
		Destroy(gameObject);
	}
}
