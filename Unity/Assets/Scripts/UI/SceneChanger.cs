using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public string SceneName;

	public void ChangeScene()
	{
		CursorManager.GetInstance().SetDefaultCursor();
		SceneManager.LoadScene(SceneName);
	}
}
