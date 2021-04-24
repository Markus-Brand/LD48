using System;
using System.Collections.Generic;
using UnityEngine;

public static class FactTopicMethods
{
	private static readonly Dictionary<FactTopic, string> DisplayTexts = new Dictionary<FactTopic, string> {
		{FactTopic.King, "King Richard III"},
		{FactTopic.KingFather, "King George IV"},
		{FactTopic.KingAdvisor, "Severin, Advisor of the King"},
	};

	public static string getDisplayText(this FactTopic topic)
	{
		return DisplayTexts[topic];
	}

	public static void checkCompleteness()
	{
		foreach (var topic in Util.getAllEnumValues<FactTopic>()) {
			if (!DisplayTexts.ContainsKey(topic)) {
				Debug.LogError("No display text for fact topic! " + topic);
			}
		}
	}
}
