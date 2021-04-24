using System;
using System.Collections.Generic;

[Serializable]
public class DialogueElement
{
	public string speaker;
	public string text;
	public List<Fact> factsToLearn;
}
