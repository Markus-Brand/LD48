
using System.Collections.Generic;
using System.Linq;

public static class LanguageUtilities
{
	private static Dictionary<Language, string> _languageTopics = new Dictionary<Language, string>(){
		{Language.Precursor, "precursorLanguage"},
		{Language.Kyr, "kyrLanguage"}
	};
	
	public static LanguageLevel GetLevel(this Language language)
	{
		if (language == Language.Precursor) {
			switch (FactManager.Instance.TopicsById[_languageTopics[language]].Facts.Count(fact => fact.IsDiscovered)) {
				case 0: return LanguageLevel.None;
				case 1: return LanguageLevel.Some;
				case 2: return LanguageLevel.Most;
				case 3: return LanguageLevel.Full;
			}
		} else if (language == Language.Kyr) {
			if (FactManager.Instance.TopicsById[_languageTopics[language]].Facts.Count(fact => fact.IsDiscovered) == 1) {
				return LanguageLevel.Full;
			} else {
				return LanguageLevel.None;
			}
		}

		return LanguageLevel.None;
	}
}
