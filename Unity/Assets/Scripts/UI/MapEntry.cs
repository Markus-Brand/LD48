using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	public Sprite HighlightSprite;
	private Sprite _nonHighlightSprite;
	private Image _image;

	public string SceneName;

	private void Start()
	{
		_image = GetComponent<Image>();
		_nonHighlightSprite = _image.sprite;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		_image.sprite = HighlightSprite;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_image.sprite = _nonHighlightSprite;
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
