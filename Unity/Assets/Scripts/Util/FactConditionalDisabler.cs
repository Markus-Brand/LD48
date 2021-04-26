using System;
using UnityEngine;
using WgEventSystem;
using WgEventSystem.Events;

public class FactConditionalDisabler : MonoBehaviour
{
	public bool Invisible;
	public bool DisableCollider = true;
	
	public FactReference VisibleOnce;
	public FactReference VisibleUntil;

	public bool Present => (VisibleOnce.IsNull || VisibleOnce.Discovered) &&
	                       (VisibleUntil.IsNull || !VisibleUntil.Discovered);

	private SpriteRenderer _spriteRenderer;
	private Collider2D _collider;
	
	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_collider = GetComponent<Collider2D>();
		UpdateVisibilty();
		EventManager.getInstance().On<FactStateChangedEvent>(e => UpdateVisibilty());
	}

	public void UpdateVisibilty()
	{
		if (Invisible && _spriteRenderer != null) _spriteRenderer.enabled = Present;
		if (DisableCollider && _collider != null) _collider.enabled = Present;
	}
}
