using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	private static readonly Color HighlightColor = Color.white.WithAlpha(0.7f);
	private static readonly Color NonHighlightColor = Color.black.WithAlpha(0.5f);
	
	private Image _image;

	public string SceneName;
	public string LocationDisplayName;
	[TextArea(3, 10)]
	public string LocationDisplayDescription;

	private MapManager Manager => transform.parent.GetComponent<MapManager>();

	private void Start()
	{
		_image = GetComponent<Image>();
		OnPointerExit(null);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_image.color = HighlightColor;
		Manager.OnHoverEntry(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_image.color = NonHighlightColor;
		Manager.OnHoverNothing();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnPointerExit(null);
		Notebook.GetInstance().Close();
		StartCoroutine(LoadScene(SceneName));
	}
	
	private static IEnumerator LoadScene(string name)
	{
		var load = SceneManager.LoadSceneAsync(name);
		while (!load.isDone) {
			yield return null;
		}
	}
}
