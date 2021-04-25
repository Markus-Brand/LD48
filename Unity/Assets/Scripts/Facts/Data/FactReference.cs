using System;
using System.Collections.Generic;

[Serializable]
public class FactReference
{
	public string FactID;

	public bool IsNull => FactID == "";
	public FactState State {
		get => !IsNull ? FactsManager.GetFactState(FactID) : FactState.Discovered;
		set => FactsManager.SetFactState(FactID, value);
	}

	public void Discover()
	{
		FactsManager.Discover(FactID);
	}
}
