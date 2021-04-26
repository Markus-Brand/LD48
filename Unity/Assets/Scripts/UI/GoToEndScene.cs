using UnityEngine;

public class GoToEndScene : MonoBehaviour
{
	public void DestroyMachine()
	{
		EndScene.ShowEndScene(false);
	}

	public void ReverseMachine()
	{
		EndScene.ShowEndScene(true);
	}
}
