using TMPro;
using UnityEngine;

public class MapManager : MonoBehaviour
{
	public string DefaultName;
	[TextArea(3, 10)]
	public string DefaultDescription;

	public TMP_Text TitleText;
	public TMP_Text DescriptionText;
	
	public void OnHoverEntry(MapEntry entry)
	{
		_showDescription(entry.LocationDisplayName, entry.LocationDisplayDescription);
	}

	public void OnHoverNothing()
	{
		_showDescription(DefaultName, DefaultDescription);
	}

	private void _showDescription(string title, string description)
	{
		TitleText.text = title;
		DescriptionText.text = description;
	}
}
