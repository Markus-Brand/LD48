using System;

[Serializable]
public class TopicReference
{
	public string TopicID;

	public bool IsNull => TopicID == "";
	public Topic Topic => FactManager.Instance.TopicsById[TopicID];
}
