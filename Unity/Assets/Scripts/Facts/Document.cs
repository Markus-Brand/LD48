using System.Collections.Generic;
using UnityEngine;

public class Document : MonoBehaviour
{
	[TextArea(3, 999)]
	public string text;
	public List<FactOccurrence> facts;
}
