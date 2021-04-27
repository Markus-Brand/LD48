namespace WgEventSystem.Events
{
	public class FactStateChangedEvent : GameEvent
	{
		public readonly string FactId;
		public readonly FactState NewState;

		public FactStateChangedEvent(string factId, FactState newState)
		{
			this.FactId = factId;
			this.NewState = newState;
		}
	}
}
