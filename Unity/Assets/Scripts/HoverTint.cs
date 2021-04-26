using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class HoverTint : MonoBehaviour
{
	private SpriteRenderer _sprite;
	private static readonly int OverlayColor = Shader.PropertyToID("_OverlayColor");

	private void Start()
	{
		_sprite = GetComponent<SpriteRenderer>();
		_sprite.material = new Material(_sprite.material);
	}

	private void OnMouseEnter()
	{
		_sprite?.material.SetColor(OverlayColor,  Color.white.WithAlpha(0.5f));
	}

	private void OnMouseExit()
	{
		Cursor.SetCursor(null, new Vector2(32, 32), CursorMode.Auto);
		_sprite?.material.SetColor(OverlayColor, Color.white.WithAlpha(0));
	}
}
