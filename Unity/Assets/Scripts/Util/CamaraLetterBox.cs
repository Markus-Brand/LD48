using System;
using UnityEngine;
using UnityEngine.UI;

public class CamaraLetterBox : MonoBehaviour
{
	public int AspectWidth = 16;
	public int AspectHeight = 9;
	
	private Camera _camera;
	private float _defaultSize;

	private void Start()
	{
		_camera = GetComponent<Camera>();
		_defaultSize = _camera.orthographicSize;
	}

	private void Update()
	{
		var targetAspect = (float)AspectWidth / AspectHeight;
		_camera.orthographicSize = _defaultSize * Math.Max(targetAspect / _camera.aspect, 1f);
	}
}
