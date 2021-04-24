public static class SpeakerMethods
{
	public static string GetDisplayName(this Speaker speaker)
	{
		return speaker.ToString().AddSpaceBetweenWords();
	}
}
