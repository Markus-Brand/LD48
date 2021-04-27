using System;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoverMaster : MonoBehaviour
{
	private static HoverMaster _instance;
	public static HoverMaster GetInstance()
	{
		if (_instance == null) {
			_instance = FindObjectOfType<HoverMaster>();
		}
		return _instance;
	}

	public enum AxisState
	{
		Normal,
		Flipped,
		Centered
	}
	
	public GameObject HoverDisplay;

	private void Awake()
	{
		_defaultTextSize = Text.fontSize;
		_canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
	}

	private void Start()
	{
		RenderPipelineManager.beginContextRendering += (ctx, cam) => {
			ReScale();
		};
		SceneManager.sceneLoaded += (e, m) => ForceHide();
	}

	private TextMeshProUGUI _text;
	private TextMeshProUGUI Text {
		get {
			if (_text != null) return _text;
			_text = HoverDisplay.GetComponentInChildren<TextMeshProUGUI>();
			return _text;
		}
	}

	private bool _visible = false;
	private float _visibleTime = 0;
	private Vector2 _lastMousePosition;
	private bool _forceHidden = false;
	
	private AxisState _xState = AxisState.Normal;
	private AxisState _yState = AxisState.Normal;

	private Transform _hoveredObject;
	public Transform HoveredObject => _hoveredObject;

	public bool HasPreferredXState;
	public AxisState PreferredXState;
	public bool HasPreferredYState;
	public AxisState PreferredYState;
	private float _defaultTextSize;
	private bool _hoveringWorldObject;
	private RectTransform _canvasTransform;

	public bool FullscreenUiOpen => Notebook.GetInstance().IsOpen() || FindObjectsOfType<DocumentOverlay>(true).Length > 0;

	private void Update()
	{
		if (FullscreenUiOpen && _hoveringWorldObject) {
			ForceHide();
		}
		
		if (_visible) _visibleTime = Math.Min(_visibleTime + Time.unscaledDeltaTime * 10, 1.0f);
		if (!_visible) _visibleTime = Math.Max(_visibleTime - Time.unscaledDeltaTime * 5, 0.0f);
		Vector2 newMousePosition = Input.mousePosition;
		if (_forceHidden || newMousePosition != _lastMousePosition) {
			ResetTimer();
			_lastMousePosition = newMousePosition;
		}

		var alpha = Math.Max(0f, Math.Min(_visibleTime, 1.0f));
		HoverDisplay.SetActive(alpha > 0.001);

		HoverDisplay.GetComponent<CanvasRenderer>().SetAlpha(alpha);
		//_text.GetComponent<CanvasRenderer>().SetAlpha(alpha);

		if (_visibleTime > 0) {
			if (_xState == AxisState.Centered && _yState == AxisState.Centered) {
				_xState = AxisState.Normal;
			}
			
			if (HasPreferredXState) {
				_xState = PreferredXState;
			}
			if (HasPreferredYState) {
				_yState = PreferredYState;
			}
			HoverDisplay.transform.localPosition = TargetPosition();
		}
	}

	private float TextPreferredWidth => (Text.preferredWidth + 20);
	private float TextPreferredHeight => (Text.preferredHeight + 20);

	private Vector2 TargetPosition()
	{
		const int minimumSpaceToScreenBorder = 20;
		const int baseOffsetFromCursor = 16;
		int offsetFromCursor = Mathf.RoundToInt(baseOffsetFromCursor);
		
		var textWidth = TextPreferredWidth;
		var textHeight = TextPreferredHeight;

		var canvasRect = _canvasTransform.rect;
		var canvasWidth = canvasRect.width;
		var canvasHeight = canvasRect.height;

		var mousePosition = Input.mousePosition;
		var relativeMouse = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
		var inCanvas = new Vector2(relativeMouse.x * canvasWidth, relativeMouse.y * canvasHeight);

		bool needsFlipX = inCanvas.x > canvasWidth - textWidth - minimumSpaceToScreenBorder;
		bool needsNoFlipX = inCanvas.x < textWidth + minimumSpaceToScreenBorder;
		if (needsFlipX && needsNoFlipX) {
			_xState = AxisState.Centered;
		} else if (needsFlipX) {
			_xState = AxisState.Flipped;
		} else if (needsNoFlipX) {
			_xState = AxisState.Normal;
		}
		
		bool needsFlipY = inCanvas.y > canvasHeight - textHeight - minimumSpaceToScreenBorder;
		bool needsNoFlipY = inCanvas.y < textHeight + minimumSpaceToScreenBorder;
		if (needsFlipY && needsNoFlipY && _xState != AxisState.Centered) {
			_yState = AxisState.Centered;
		} else if (needsFlipY) {
			_yState = AxisState.Flipped;
		} else if (needsNoFlipY) {
			_yState = AxisState.Normal;
		}


		switch (_xState) {
			case AxisState.Normal:
				inCanvas.x += offsetFromCursor;
				break;
			case AxisState.Flipped:
				inCanvas.x -= textWidth + offsetFromCursor;
				break;
			case AxisState.Centered:
				inCanvas.x -= textWidth / 2;
				break;
		}
		
		switch (_yState) {
			case AxisState.Normal:
				inCanvas.y += offsetFromCursor;
				break;
			case AxisState.Flipped:
				inCanvas.y -= textHeight + offsetFromCursor;
				break;
			case AxisState.Centered:
				inCanvas.y -= textHeight / 2;
				break;
		}
		
		return inCanvas;
	}

	public void ShowInfo(IHoverInfo hoverInfo)
	{
		if (FullscreenUiOpen && !(hoverInfo is UIHoverInfo)) return;
		_hoveredObject = hoverInfo.GetTransform();
		_hoveringWorldObject = !(hoverInfo is UIHoverInfo);
		Text.text = hoverInfo.GetHoverText().Replace("\\n", "\n");
		_forceHidden = false;

		HoverDisplay.transform.localPosition = TargetPosition();
		_visible = true;
		ResetTimer();
		ReScale();
	}

	public void ForceHide()
	{
		_forceHidden = true;
	}

	private void ResetTimer()
	{
		if (_forceHidden || _visibleTime <= 0.001) _visibleTime = -2f;
	}

	private void ReScale()
	{
		if (HoverDisplay == null) return;
		HoverDisplay.GetComponent<RectTransform>()
			.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TextPreferredWidth);
		HoverDisplay.GetComponent<RectTransform>()
			.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TextPreferredHeight);
	}

	public void HideInfo(IHoverInfo hoverInfo)
	{
		if (_hoveredObject != hoverInfo.GetTransform()) return;
		_hoveredObject = null;
		_visible = false;
	}
}
