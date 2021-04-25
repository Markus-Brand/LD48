using System;
using WgEventSystem.Events;

namespace WgEventSystem
{
	public abstract class EventListener {}
	
	public class EventListener<TEvent> : EventListener where TEvent : GameEvent
	{
		public EventListener(Action<TEvent> action, Predicate<TEvent> predicate)
		{
			_action = action;
			_predicate = predicate;
		}

		internal Action<TEvent> _action;
		private readonly Predicate<TEvent> _predicate;

		public virtual void Execute(TEvent @event)
		{
			if (_predicate == null || _predicate(@event)) {
				_action(@event);
			}
		}
	}
}
