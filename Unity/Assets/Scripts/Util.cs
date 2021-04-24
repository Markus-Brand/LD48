using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class Util
{
	public static T[] getAllEnumValues<T>() where T : struct
	{
		return (T[]) Enum.GetValues(typeof(T));
	}
	
	public static string AddSpaceBetweenWords(this string name)
	{
		return String.Join(" ", Regex.Split(name, "(?:[\\W_]+|(?<![A-Z])(?=[A-Z])|(?<!^)(?=[A-Z][a-z]))"));
	}

}
