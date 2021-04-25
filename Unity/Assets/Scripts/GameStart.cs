using EventSystem;
using EventSystem.Events;
using UnityEngine;

public class GameStart : MonoBehaviour
{
	private void Start()
	{
#if UNITY_EDITOR
		FactMethods.CheckCompleteness();
		FactTopicMethods.CheckCompleteness();
		FactMethods.GeneratePuml();
#endif
		
		EventManager.getInstance().On<FactStateChangedEvent>(e =>
			Debug.Log(Notebook.GetNotebookFactText()));
	}
}
