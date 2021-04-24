using EventSystem;
using EventSystem.Events;
using UnityEngine;

public class GameStart : MonoBehaviour
{
	private void Start()
	{
		FactMethods.CheckCompleteness();
		FactTopicMethods.CheckCompleteness();
		FactMethods.GeneratePuml();
		
		EventManager.getInstance().On<FactStateChangedEvent>(e =>
			Debug.Log(Notebook.getNotebookFactText()));
	}
}
