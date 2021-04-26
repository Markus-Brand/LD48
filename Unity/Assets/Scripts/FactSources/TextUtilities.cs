
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = System.Random;

public class TextUtilities
{

	private static readonly Regex _factRegex = new Regex(@"<fact=(\w*)>(.*?)</fact>", RegexOptions.Singleline);
	private static readonly Regex _languageRegex = new Regex(@"<lang=(\w*)>(.*?)</lang>", RegexOptions.Singleline);

	private static Dictionary<Regex, int> _illegibleRegexes = new Dictionary<Regex, int> {
		{_factRegex, 2}
	};
	
	private static Dictionary<LanguageLevel, double> _readableFractions = new Dictionary<LanguageLevel, double> {
		{LanguageLevel.None, 0.0}, {LanguageLevel.Some, 0.3}, {LanguageLevel.Most, 0.6}
	};
	
	private static Dictionary<Language, string> _languageAlphabets = new Dictionary<Language, string> {
		{Language.Precursor, "abcdefghijklmnopqrstuvwxyz()$!? "}, {Language.Kyr, "cdefghijklmnopqrstuvwxyz:.,\"'@/     "}
	};
	
	public static string TransformText(string text)
	{
		text = _languageRegex.Replace(text, match => {
			var language = Util.parse<Language>(match.Groups[1].Value);
			return TransformLanguage(match.Groups[2].Value, language);
		});
		
		return _factRegex.Replace(text, match => {
			var factID = match.Groups[1].Value;
			if (FactManager.GetFactState(factID) != FactState.Undiscovered) {
				return $"<mark>{match.Groups[2].Value}</mark>";
			} else {
				return $"<link=unlock-fact:{match.Groups[1].Value}>{match.Groups[2].Value}</link>";
			}
		});
	}

	public static string TransformLanguage(string content, Language language)
	{
		var skill = language.GetLevel();
		if (skill == LanguageLevel.Full) return content;
		foreach (var regex in _illegibleRegexes) {
			content = regex.Key.Replace(content, $"${regex.Value.ToString()}");
		}

		var readableFraction = _readableFractions[skill];
		var alphabet = _languageAlphabets[language];
		var r = new Random(content.GetHashCode());

		StringBuilder output = new StringBuilder();
		
		foreach (var c in content) {
			var rD = r.NextDouble();
			var rI = r.Next();
			if (rD < readableFraction) {
				output.Append(c);
				continue;
			}
			if (c == '\n') {
				output.Append(c);
				continue;
			}

			var n = alphabet[(c + rI) % alphabet.Length];
			output.Append($"<font=\"{language.ToString()}\">{n.ToString()}</font>");
		}
		return output.ToString();
	}

	public static List<string> GetFactIdsInText(string text)
	{
		var matches = _factRegex.Matches(text);
		var results = new List<string>();
		for (int i = 0; i < matches.Count; i++) {
			results.Add(matches[i].Groups[1].Value);
		}
		return results;
	}
}
