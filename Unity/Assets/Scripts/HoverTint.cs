using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class HoverTint : MonoBehaviour
{
	private SpriteRenderer _sprite;
	private static readonly int OverlayColor = Shader.PropertyToID("_OverlayColor");

	public bool UseInvestigationCursor = false;
	
	private void Start()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_sprite.material = new Material(_sprite.material);
	}

	private void OnMouseEnter()
	{
		var dp = GetComponent<DialoguePerson>();
		if (dp != null) {
			if(!dp.HasOptions()) return;
		}
		if (HoverMaster.GetInstance().FullscreenUiOpen) return;
		if(UseInvestigationCursor) CursorManager.GetInstance().SetInvestigateCursor();
		_sprite?.material.SetColor(OverlayColor,  Color.white.WithAlpha(0.5f));
	}

	private void OnMouseExit()
	{
		if(UseInvestigationCursor) CursorManager.GetInstance().SetDefaultCursor();
		_sprite?.material.SetColor(OverlayColor, Color.white.WithAlpha(0));
	}
}
