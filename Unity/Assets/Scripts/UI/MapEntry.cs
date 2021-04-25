using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WgEventSystem;
using WgEventSystem.Events;

public class MapEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	private static readonly Color HighlightColor = Color.white.WithAlpha(0.7f);
	private static readonly Color NonHighlightColor = Color.black.WithAlpha(0.5f);
	private static readonly Color UndiscoveredColor = Color.black.WithAlpha(0.2f);
	
	private Image _image;

	public string SceneName;
	public string LocationDisplayName;
	[TextArea(3, 10)]
	public string LocationDisplayDescription;
	public FactReference UnlockCondition;

	private MapManager Manager => transform.parent.GetComponent<MapManager>();

	private void Start()
	{
		_image = GetComponent<Image>();
		ResetAppearance();
		EventManager.getInstance().On<FactStateChangedEvent>(e => ResetAppearance());
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!UnlockCondition.Discovered) return;
		_image.color = HighlightColor;
		Manager.OnHoverEntry(this);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (!UnlockCondition.Discovered) return;
		ResetAppearance();
		Manager.OnHoverNothing();
	}

	private void ResetAppearance()
	{
		if (UnlockCondition.Discovered) {
			_image.color = NonHighlightColor;
		} else {
			_image.color = UndiscoveredColor;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!UnlockCondition.Discovered) return;
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
