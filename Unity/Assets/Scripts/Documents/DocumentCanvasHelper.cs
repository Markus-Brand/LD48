using System;
using UnityEngine;

namespace Documents
{
	public class DocumentCanvasHelper : MonoBehaviour
	{
		private void Awake()
		{
			GetComponent<Canvas>().worldCamera = Camera.main;
		}
	}
}
