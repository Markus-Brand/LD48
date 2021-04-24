using System;
using EventSystem.Events;
using Object = UnityEngine.Object;

namespace EventSystem
{
	public class LifetimeConstrainedEventListener<TEvent> : EventListener<TEvent> where TEvent : GameEvent
	{
		public LifetimeConstrainedEventListener(Action<TEvent> action, Predicate<TEvent> predicate,
			Object lifetimeReference, Action<EventListener<TEvent>> selfDelete) : base(action, predicate)
		{
			_lifetimeReference = lifetimeReference;
			_selfDelete = selfDelete;
		}

		private readonly Object _lifetimeReference;
		private readonly Action<EventListener<TEvent>> _selfDelete;

		public override void Execute(TEvent @event)
		{
			if (_lifetimeReference == null) {
				_selfDelete(this);
			} else {
				base.Execute(@event);				
			}
		}
	}
}
