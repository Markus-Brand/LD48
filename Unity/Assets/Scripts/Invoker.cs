/*
Copyright (c) 2015 Funonium (Jade Skaggs)
	
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

using UnityEngine;
using System.Collections.Generic;


public delegate void Invokable();

/// <summary>
/// Enables invokation of functions without regard to timeScale
/// To use this class, Call Invoker.InvokeDelayed(MyFunc, 5);
/// 
/// Written by Jade Skaggs - Funonium.com
/// </summary>
public class Invoker : MonoBehaviour {
	private struct InvokableItem
	{
		public readonly Invokable Func;
		public readonly float ExecuteAtTime;

		public InvokableItem(Invokable func, float delaySeconds, bool scaled = false)
		{
			Func = func;

			if (scaled) {
				ExecuteAtTime = Time.time + delaySeconds;
			} else {
				// realtimeSinceStartup is never 0, and Time.time is affected by timescale (though it is 0 on startup).  Use a combination 
				// http://forum.unity3d.com/threads/205773-realtimeSinceStartup-is-not-0-in-first-Awake()-call
				if (Time.time == 0)
					ExecuteAtTime = delaySeconds;
				else
					ExecuteAtTime = Time.realtimeSinceStartup + delaySeconds;
			}
		}
	}
	
	private static Invoker _instance = null;
	public static Invoker Instance
	{
		get {
			if (_instance == null) {
				var go = new GameObject("ACSInvoker");
				go.AddComponent<Invoker>();
				_instance = go.GetComponent<Invoker>();
			}
			return _instance;
		}
	}

	private readonly List<InvokableItem> _invokeList = new List<InvokableItem>();
	private readonly List<InvokableItem> _invokeListPendingAddition = new List<InvokableItem>();
	
	private readonly List<InvokableItem> _timeScaledInvokeList = new List<InvokableItem>();
	private readonly List<InvokableItem> _timeScaledInvokeListPendingAddition = new List<InvokableItem>();
	
	/// <summary>
	/// Invokes the function with a time delay.  This is NOT 
	/// affected by timeScale like the Invoke function in Unity.
	/// </summary>
	/// <param name='func'>
	/// Function to invoke
	/// </param>
	/// <param name='delaySeconds'>
	/// Delay in seconds.
	/// </param>
	public static void InvokeUnscaled(Invokable func, float delaySeconds)
	{
		Instance._invokeListPendingAddition.Add(new InvokableItem(func, delaySeconds));
	}

	public static void InvokeScaled(Invokable func, float delaySeconds)
	{
		Instance._timeScaledInvokeListPendingAddition.Add(new InvokableItem(func, delaySeconds, true));
	}
	
	// must be manually called from a game controller or something similar every frame
	public void Update()
	{	
		// Copy pending additions into the list (Pending addition list 
		// is used because some invokes add a recurring invoke, and
		// this would modify the collection in the next loop, 
		// generating errors)
		_invokeList.AddRange(_invokeListPendingAddition);
		_invokeListPendingAddition.Clear();
		_timeScaledInvokeList.AddRange(_timeScaledInvokeListPendingAddition);
		_timeScaledInvokeListPendingAddition.Clear();
		
		
		// Invoke all items whose time is up
		var executed = new List<InvokableItem>();
		foreach (var item in _invokeList) {
			if (item.ExecuteAtTime <= Time.realtimeSinceStartup) {
				item.Func?.Invoke();
				executed.Add(item);
			}
		}
		foreach (var item in _timeScaledInvokeList) {
			if (item.ExecuteAtTime <= Time.time) {
				item.Func?.Invoke();
				executed.Add(item);
			}
		}
		
		
		// Remove invoked items from the list.
		_invokeList.RemoveAll(executed.Contains);
		_timeScaledInvokeList.RemoveAll(executed.Contains);
		executed.Clear();
	}
}