using EventSystem;
using EventSystem.Events;
using UnityEngine;

public class GameStart : MonoBehaviour
{
	private void Start()
	{
		FactMethods.checkCompleteness();
		FactTopicMethods.checkCompleteness();
		
		EventManager.getInstance().On<FactStateChangedEvent>(e =>
			Debug.Log(Notebook.getNotebookFactText()));
	}
}
