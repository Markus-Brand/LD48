using System.Collections.Generic;
using System.Linq;

public class Notebook
{
	public static string getNotebookFactText()
	{
		string result = "";

		foreach (var topic in Util.getAllEnumValues<FactTopic>()) {
			result += topic.getDisplayText() + "\n\n";
			foreach (var fact in getDiscoveredFactsOfTopic(topic)) {
				result += "- " + fact.getDisplayText();
			}
		}

		return result;
	}

	private static IEnumerable<Fact> getDiscoveredFactsOfTopic(FactTopic topic)
	{
		return Util.getAllEnumValues<Fact>()
			.Where(fact => fact.getState() == FactState.Discovered && fact.getTopic() == topic);
	}
}
