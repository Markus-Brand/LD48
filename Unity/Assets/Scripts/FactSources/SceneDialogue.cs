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

	//TODO: If we need another SceneDialogue, this logic needs to find a new home
	public void DialogueDone()
	{
		SceneManager.LoadScene("Throne Room");
	}
}
