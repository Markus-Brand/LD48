using System;
using System.Collections.Generic;
using EventSystem.Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace EventSystem
{
	public class EventManager : MonoBehaviour
	{
		private static EventManager _singleton = null;
		public static EventManager getInstance()
		{
			if (_singleton == null) {
				GameObject o = new GameObject("EventManager");
				DontDestroyOnLoad(o);
				_singleton = o.AddComponent<EventManager>();
			}
			return _singleton;
		}

		private readonly Dictionary<Type, EventWrapper> _events = new Dictionary<Type, EventWrapper>();

		private EventWrapper<T> CreateEventWrapper<T>() where T : GameEvent
		{
			return new EventWrapper<T>();
		}
		
		private EventWrapper<T> GetEventWrapper<T>() where T : GameEvent
		{
			EventWrapper genericWrapper;
			if (_events.TryGetValue(typeof(T), out genericWrapper)) {
				return (EventWrapper<T>) genericWrapper;
			}
			EventWrapper<T> eventWrapper = CreateEventWrapper<T>();
			_events.Add(typeof(T), eventWrapper);
			return eventWrapper;
		}

		public void AddListener<T>(EventListener<T> listener) where T : GameEvent
		{
			GetEventWrapper<T>().AddListener(listener);
		}
		
		public void RemoveListener<T>(EventListener<T> listener) where T : GameEvent
		{
			GetEventWrapper<T>().RemoveListener(listener);
		}
		
		public EventListener<T> On<T>(Action<T> action, Predicate<T> predicate = null) where T : GameEvent
		{
			var listener = new EventListener<T>(action, predicate);
			AddListener(listener);
			return listener;
		}
		
		public EventListener<T> On<T>(Object lifetimeReference, Action<T> action, Predicate<T> predicate = null) where T : GameEvent
		{
			var listener = new LifetimeConstrainedEventListener<T>(action, predicate, lifetimeReference, RemoveListener);
			AddListener(listener);
			return listener;
		}
		
		public EventListener<T> Once<T>(Action<T> action, Predicate<T> predicate = null) where T : GameEvent
		{
			var listener = new EventListener<T>(null, predicate);
			
			//TODO: Make this niciger and more genericiger, like only trigger 5 times
			listener._action = delegate(T @event) {
				action(@event);
				RemoveListener(listener);
			};
			
			AddListener(listener);
			return listener;
		}
		
		public EventListener<T> Once<T>(Object lifetimeReference, Action<T> action, Predicate<T> predicate = null) where T : GameEvent
		{
			var listener = new LifetimeConstrainedEventListener<T>(null, predicate, lifetimeReference, RemoveListener);
			
			//TODO: Make this niciger and more genericiger, like only trigger 5 times
			listener._action = delegate(T @event) {
				action(@event);
				RemoveListener(listener);
			};
			
			AddListener(listener);
			return listener;
		}

		public void Trigger<T>(T @event) where T : GameEvent
		{
			GetEventWrapper<T>().Trigger(@event);
		}
	}
}
