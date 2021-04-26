using WgEventSystem;
using WgEventSystem.Events;
using UnityEngine;

public class GameStart : MonoBehaviour
{
	private void Start()
	{
		EventManager.getInstance().On<FactStateChangedEvent>(e =>
			Debug.Log(Notebook.GetNotebookFactText()));
	}
}
