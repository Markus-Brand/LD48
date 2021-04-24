using System;

public static class Util
{
	public static T[] getAllEnumValues<T>() where T : struct
	{
		return (T[]) Enum.GetValues(typeof(T));
	}
}
