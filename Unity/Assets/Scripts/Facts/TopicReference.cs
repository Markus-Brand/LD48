using System;

[Serializable]
public class TopicReference
{
	public string TopicID;

	public bool IsNull => TopicID == "";
	private Topic Topic => FactManager.Instance.TopicsById.GetOrDefault(TopicID);

	public string CurrentName => Topic?.CurrentName ?? "Unnamed";
}
