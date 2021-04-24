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
			result += topic.getDisplayText() + "\n";
			foreach (var fact in discoveredFactsOfTopic) {
				result += "- " + fact.getDisplayText() + "\n";
			}
			result += "\n";
		}

		return result;
	}

	private static List<Fact> getDiscoveredFactsOfTopic(FactTopic topic)
	{
		return Util.getAllEnumValues<Fact>()
			.Where(fact => fact.getState() == FactState.Discovered && fact.getTopic() == topic).ToList();
	}
}
