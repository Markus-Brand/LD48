using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.IO;
#endif
using System.Linq;
using WgEventSystem;
using WgEventSystem.Events;
using UnityEngine;

public static class FactMethods
{
	private static readonly Dictionary<Fact, string> DisplayTexts = new Dictionary<Fact, string> {
		{
			Fact.KingWantsYourHelpWithFathersDeath,
			"King Richard III needs your help! His fathers death was somewhat suspicious."
		},
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

	private static readonly Fact[] None = { }; 
	// all of these condition facts need to be discovered so that the key fact can also be discovered
	private static readonly Dictionary<Fact, Fact[]> Conditions = new Dictionary<Fact, Fact[]> {
		{Fact.KingWantsYourHelpWithFathersDeath, None},
		{Fact.KingsFatherDiedMysteriously, None},
		{Fact.KingsFatherHadWeirdSymptoms, new[] {Fact.KingsFatherDiedMysteriously, Fact.KingWantsYourHelpWithFathersDeath}},
		{Fact.KingsFatherWasPoisoned, new[] {Fact.KingsFatherHadWeirdSymptoms}},
		{Fact.KingsAdvisorBehavesWeird, new[] {Fact.KingsFatherWasPoisoned}},
	};

	public static string GetDisplayText(this Fact fact)
	{
		return DisplayTexts[fact];
	}

	public static FactTopic GetTopic(this Fact fact)
	{
		return Topics[fact];
	}

	private static readonly Dictionary<Fact, FactState> states = new Dictionary<Fact, FactState>();

	public static FactState GetState(this Fact fact)
	{
		return states.ContainsKey(fact) ? states[fact] : FactState.Undiscovered;
	}

	public static bool Discover(this Fact fact, bool force = false)
	{
		if (fact.GetState() != FactState.Undiscovered) return false;
		if (!force) {
			if (Conditions[fact].Any(cond => cond.GetState() != FactState.Discovered)) {
				return false;
			}
		}
		states[fact] = FactState.Discovered;
		Debug.Log("Just discovered: " + fact);
		EventManager.getInstance().Trigger(new FactStateChangedEvent());
		return true;
	}

#if UNITY_EDITOR
	public static void CheckCompleteness()
	{
		foreach (var fact in Util.getAllEnumValues<Fact>()) {
			if (!DisplayTexts.ContainsKey(fact)) {
				Debug.LogError("No description for fact! " + fact);
			}
			if (!Topics.ContainsKey(fact)) {
				Debug.LogError("No topic for fact! " + fact);
			}
			if (!Conditions.ContainsKey(fact)) {
				Debug.LogError("No topic for fact! " + fact);
			}
		}
	}

	public static void GeneratePuml()
	{
		string content = "@startuml\ndigraph facts {\n\n";
		foreach (var fact in Util.getAllEnumValues<Fact>()) {
			content += fact + " [label =\"" + fact + "\n" + fact.GetDisplayText() + "\"]\n";
		}
		content += "\n";
		foreach (var fact in Util.getAllEnumValues<Fact>()) {
			foreach (var cond in Conditions[fact]) {
				content += cond + " -> " + fact + "\n";
			}
		}
		content += "\n}\n@enduml";

		string path = "Assets" + Path.DirectorySeparatorChar + "fact-diagram.puml";
		using (var sw = File.CreateText(path)) {
			sw.Write(content);
		}
	}
#endif
}
