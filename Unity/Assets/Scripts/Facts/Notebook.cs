using System.Collections.Generic;
using System.Linq;

public class Notebook
{
	public static string getNotebookFactText()
	{
		string result = "";

		foreach (var topic in Util.getAllEnumValues<FactTopic>()) {
			var discoveredFactsOfTopic = getDiscoveredFactsOfTopic(topic);
			if (discoveredFactsOfTopic.Count == 0) continue;
			result += topic.GetDisplayText() + "\n";
			foreach (var fact in discoveredFactsOfTopic) {
				result += "- " + fact.GetDisplayText() + "\n";
			}
			result += "\n";
		}

		return result;
	}

	private static List<Fact> getDiscoveredFactsOfTopic(FactTopic topic)
	{
		return Util.getAllEnumValues<Fact>()
			.Where(fact => fact.GetState() == FactState.Discovered && fact.GetTopic() == topic).ToList();
	}
}
