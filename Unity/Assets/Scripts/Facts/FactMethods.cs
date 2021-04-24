using System;
using System.Collections.Generic;
using UnityEngine;

public static class FactMethods
{
	private static readonly Dictionary<Fact, string> DisplayTexts = new Dictionary<Fact, string> {
		{Fact.KingWantsYourHelpWithFathersDeath, ""},
		{Fact.KingsFatherDiedMysteriously, ""},
		{Fact.KingsFatherHadWeirdSymptoms, ""},
		{Fact.KingsFatherWasPoisoned, ""},
		{Fact.KingsAdvisorBehavesWeird, ""},
	};
	
	private static readonly Dictionary<Fact, FactTopic> Topics = new Dictionary<Fact, FactTopic> {
		{Fact.KingWantsYourHelpWithFathersDeath, FactTopic.King},
		{Fact.KingsFatherDiedMysteriously, FactTopic.KingFather},
		{Fact.KingsFatherHadWeirdSymptoms, FactTopic.KingFather},
		{Fact.KingsFatherWasPoisoned, FactTopic.KingFather},
		{Fact.KingsAdvisorBehavesWeird, FactTopic.KingAdvisor},
	};

	public static string getDisplayText(this Fact fact)
	{
		return DisplayTexts[fact];
	}

	public static FactTopic getTopic(this Fact fact)
	{
		return Topics[fact];
	}

	private static readonly Dictionary<Fact, FactState> states = new Dictionary<Fact, FactState>();

	public static FactState getState(this Fact fact)
	{
		return states.ContainsKey(fact) ? states[fact] : FactState.Undiscovered;
	}

	public static void setState(this Fact fact, FactState newState)
	{
		states[fact] = newState;
	}

	public static void checkCompleteness()
	{
		foreach (var fact in Util.getAllEnumValues<Fact>()) {
			if (!DisplayTexts.ContainsKey(fact)) {
				Debug.LogError("No description for fact! " + fact);
			}
			if (!Topics.ContainsKey(fact)) {
				Debug.LogError("No topic for fact! " + fact);
			}
		}
	}
}
