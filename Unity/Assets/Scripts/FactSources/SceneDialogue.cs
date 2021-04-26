using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(DialogueOption))]
public class SceneDialogue : MonoBehaviour
{
	private static readonly HashSet<string> _triggeredScenes = new HashSet<string>();
	
	public void Start()
	{
		var sceneName = SceneManager.GetActiveScene().name;
		if (_triggeredScenes.Contains(sceneName)) return;
		
		_triggeredScenes.Add(sceneName);
		var option = GetComponent<DialogueOption>();
		Invoker.InvokeUnscaled(() => option.Execute(), 0.1f);
		// option.Execute();
	}

	public void DialogueDone()
	{
		SceneManager.LoadScene("Throne Room");
	}
}
