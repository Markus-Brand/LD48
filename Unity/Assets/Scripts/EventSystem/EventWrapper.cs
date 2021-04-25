using UnityEngine.Events;
using WgEventSystem.Events;

namespace WgEventSystem
{
	public abstract class EventWrapper
	{
		
	}

	public class EventWrapper<TEvent> : EventWrapper where TEvent : GameEvent
	{
		private class InternalEvent : UnityEvent<TEvent>
		{}
		
		private UnityEvent<TEvent> _event = new InternalEvent();

		public void AddListener(EventListener<TEvent> listener)
		{
			_event.AddListener(listener.Execute);
		}

		public void RemoveListener(EventListener<TEvent> listener)
		{
			_event.RemoveListener(listener.Execute);
		}

		public void Trigger(TEvent @event)
		{
			_event.Invoke(@event);
		}
	}
}
