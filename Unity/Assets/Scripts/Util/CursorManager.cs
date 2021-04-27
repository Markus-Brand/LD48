
using UnityEngine;

public class CursorManager: MonoBehaviour
{
	public static CursorManager GetInstance()
	{
		if (_instance == null) {
			_instance = FindObjectOfType<CursorManager>();
		}
		return _instance;
	}
	
	public Texture2D defaultCursor;
	public Texture2D talkCursor;
	public Texture2D investigateCursor;
	private static CursorManager _instance;

	public void SetDefaultCursor()
	{
		Cursor.SetCursor(defaultCursor, new Vector2(0, 0), CursorMode.Auto);
	}
	public void SetTalkCursor()
	{
		Cursor.SetCursor(talkCursor, new Vector2(16, 52), CursorMode.Auto);
	}
	public void SetInvestigateCursor()
	{
		Cursor.SetCursor(investigateCursor, new Vector2(16, 16), CursorMode.Auto);
	}

}
