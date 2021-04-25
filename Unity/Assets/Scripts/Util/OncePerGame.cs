using UnityEngine;

public class OncePerGame : MonoBehaviour
{
	private static OncePerGame _instance;

	private void Awake()
	{
		if (_instance != null && _instance != this) {
			Destroy(gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(gameObject);
	}
}
