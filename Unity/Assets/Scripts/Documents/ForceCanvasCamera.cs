using System;
using UnityEngine;

namespace Documents
{
	public class ForceCanvasCamera : MonoBehaviour
	{
		private void Awake()
		{
			GetComponent<Canvas>().worldCamera = Camera.main;
		}
	}
}
