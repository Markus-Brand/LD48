using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Util
{
	public static T[] getAllEnumValues<T>() where T : struct
	{
		return (T[]) Enum.GetValues(typeof(T));
	}

	public static T parse<T>(string input) where T : struct
	{
		return (T) Enum.Parse(typeof(T), input);
	}

	public static string AddSpaceBetweenWords(this string name)
	{
		return String.Join(" ", Regex.Split(name, "(?:[\\W_]+|(?<![A-Z])(?=[A-Z])|(?<!^)(?=[A-Z][a-z]))"));
	}
	
	public static Color WithAlpha(this Color color, float alpha)
	{
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static float SmoothToOne(float raw, int amount = 3)
	{
		var inv = 1.0f - raw;
		var smooth = Mathf.Pow(inv, amount);
		return 1.0f - smooth;
	}

	public static bool Equalish(this float a, float b, float tolerance = 0.1f)
	{
		return Mathf.Abs(a - b) <= tolerance;
	}

	public static bool Equalish(this Vector2 a, Vector2 b, float tolerance = 0.001f)
	{
		return Mathf.Abs((a - b).sqrMagnitude) <= tolerance;
	}

	public static bool Equalish(this Vector3 a, Vector3 b, float tolerance = 0.001f)
	{
		return Mathf.Abs((a - b).sqrMagnitude) <= tolerance;
	}

	public static TValue GetOrDefault<TKey, TValue>
	(this IDictionary<TKey, TValue> dictionary,
		TKey key,
		TValue defaultValue = default)
	{
		TValue value;
		return dictionary.TryGetValue(key, out value) ? value : defaultValue;
	}
}
