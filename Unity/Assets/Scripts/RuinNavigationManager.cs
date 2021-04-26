using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class RuinNavigationManager : MonoBehaviour
{
	private static RuinNavigationManager _instance;

	public RuinNavigationManager GetInstance()
	{
		if (_instance == null) {
			_instance = FindObjectOfType<RuinNavigationManager>();
		}
		return _instance;
	}

	public GameObject OutsideStuff;
	public GameObject InsideStuff;
	public GameObject TargetRoomStuff;
	public SpriteRenderer SymbolRenderer;
	public SpriteRenderer FadeToBlack;
	
	public List<Sprite> SymbolSprites;

	private readonly SmoothToggle _blackFaded = new SmoothToggle(false, 0.4f);

	private List<bool> _navigations = null;
	private bool IsOutside => _navigations == null;

	private void Start()
	{
		SetupSceneFromNavigation();
		FadeToBlack.gameObject.SetActive(true);
	}

	private void Update()
	{
		_blackFaded.Update();
		if (_blackFaded.IsTrue()) {
			SetupSceneFromNavigation();
			_blackFaded.SetFalse();
		}
		FadeToBlack.color = FadeToBlack.color.WithAlpha(_blackFaded.CurrentValue);
	}

	public void OnEnterRuins()
	{
		_navigations = new List<bool>();
		_blackFaded.SetTrue();
		//SetupSceneFromNavigation();
	}

	public void OnNavigateLeft()
	{
		OnNavigate(true);
	}

	public void OnNavigateRight()
	{
		OnNavigate(false);
	}

	private void OnNavigate(bool direction)
	{
		if (_blackFaded.IsOrBecomesTrue()) return; // we are already interacting!
		_navigations.Add(direction);
		if (_navigations.Count > 6) {
			_navigations = null;
		}
		_blackFaded.SetTrue();
	}

	private void SetupSceneFromNavigation()
	{
		var outside = IsOutside;
		var targetReached = !outside && _navigations.SequenceEqual(new[] {true, true, false, false, true, true});
		OutsideStuff.SetActive(outside);
		InsideStuff.SetActive(!outside && !targetReached);
		TargetRoomStuff.SetActive(!outside && targetReached);
		if (!outside &&!targetReached) {
			var r = new Random(HashState() + 1);
			var spriteIndex = r.Next() % SymbolSprites.Count;
			var rotation = (r.Next() % 4) * 90;
			var flipX = r.NextDouble() < 0.5f;
			SymbolRenderer.sprite = SymbolSprites[spriteIndex];
			SymbolRenderer.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
			SymbolRenderer.flipX = flipX;
		}
	}

	private int HashState()
	{
		int result = 30930145; // true random, decided by fair dice roll
		foreach (var direction in _navigations) {
			result = result * 31 + (direction ? 1 : 0);
		}
		return result;
	}
}
