using WgEventSystem;
using WgEventSystem.Events;
using UnityEngine;

public class GameStart : MonoBehaviour
{
	private void Start()
	{
#if UNITY_EDITOR
		// FactManager.GeneratePuml();
#endif
		
		EventManager.getInstance().On<FactStateChangedEvent>(e =>
			Debug.Log(Notebook.GetNotebookFactText()));
	}
}
