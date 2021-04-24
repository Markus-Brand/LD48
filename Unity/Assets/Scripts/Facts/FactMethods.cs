using System;
using System.Collections.Generic;
using EventSystem;
using EventSystem.Events;
using UnityEngine;

public static class FactMethods
{
	private static readonly Dictionary<Fact, string> DisplayTexts = new Dictionary<Fact, string> {
		{Fact.KingWantsYourHelpWithFathersDeath, "King Richard III needs your help! His fathers death was somewhat suspicious."},
		{Fact.KingsFatherDiedMysteriously, "Dies under mysterious circumstances."},
		{Fact.KingsFatherHadWeirdSymptoms, "Had weird symptoms shortly before his death TODO WHICH SYMPTOMS?"},
		{Fact.KingsFatherWasPoisoned, "Was probably poisoned to death"},
		{Fact.KingsAdvisorBehavesWeird, "Did not quite like to investigate the death of King George IV further"},
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
		if (newState == fact.getState()) return;
		states[fact] = newState;
		EventManager.getInstance().Trigger(new FactStateChangedEvent());
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
