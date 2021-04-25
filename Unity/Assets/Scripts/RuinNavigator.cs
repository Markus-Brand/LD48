using System;
using UnityEngine;
using UnityEngine.Events;

public class RuinNavigator : MonoBehaviour
{
	public UnityEvent OnEnter;
	private void OnMouseDown()
	{
		OnEnter.Invoke();
	}
}
