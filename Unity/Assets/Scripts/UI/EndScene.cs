using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndScene : MonoBehaviour
{
	private static bool _rescuedPrecursors = false;

	public static void ShowEndScene(bool rescuedPrecursors)
	{
		OncePerGame.DestroyInstance();
		_rescuedPrecursors = rescuedPrecursors;
		Debug.Log(_rescuedPrecursors);
		SceneManager.LoadScene("End");
	}

	[TextArea(20, 100)]
	public string RescueEndText;
	[TextArea(20, 100)]
	public string DestroyEndText;

	public TMP_Text Text;

	private void Start()
	{
		Debug.Log(_rescuedPrecursors);
		Text.text = _rescuedPrecursors ? RescueEndText : DestroyEndText;
	}
}
