using System;
using System.Collections.Generic;

[Serializable]
public class FactReference
{
	public string FactID;

	public bool IsNull => string.IsNullOrEmpty(FactID);
	public FactState State {
		get => !IsNull ? FactManager.GetFactState(FactID) : FactState.Discovered;
		set => FactManager.SetFactState(FactID, value);
	}
	
	public bool Discovered => State == FactState.Discovered;

	public void Discover()
	{
		FactManager.Discover(FactID);
	}

	public void Discard()
	{
		FactManager.Discard(FactID);
	}

}
