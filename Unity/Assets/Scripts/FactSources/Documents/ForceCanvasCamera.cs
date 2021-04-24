using System;
using UnityEngine;

public class ForceCanvasCamera : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Canvas>().worldCamera = Camera.main;
	}
}
