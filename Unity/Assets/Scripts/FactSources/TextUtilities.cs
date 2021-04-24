
using System;
using System.Text.RegularExpressions;

public class TextUtilities
{

	private static Regex _factRegex = new Regex("<fact=(\\w*)>(.*?)</fact>");
	
	public static string TransformText(string text)
	{
		return _factRegex.Replace(text, match => {
			var fact = (Fact)Enum.Parse(typeof(Fact), match.Groups[1].Value);
			if (fact.GetState() != FactState.Undiscovered) {
				return $"<mark>{match.Groups[2].Value}</mark>";
			} else {
				return $"<link=unlock-fact:{match.Groups[1].Value}>{match.Groups[2].Value}</link>";
			}
		});
	}
}
