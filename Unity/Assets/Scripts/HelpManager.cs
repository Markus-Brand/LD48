using System.Linq;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
	private static DialogueManager Dm => DialogueManager.GetInstance();
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H)) {
			if (!Dm.IsCurrentlyActive) {
				var discoverableFacts = FactManager.Instance.AllDiscoverableFacts.Where(f => f.HelpText.Length > 0).ToList();
				if (discoverableFacts.Count == 0) {
					ShowHelp("I just don't know what to do now...");
				} else {
					ShowHelp(discoverableFacts[0].HelpText);
				}
			} else if (Dm.ShowsHelpText) {
				Dm.Continue();
			}
		}
	}

	private void ShowHelp(string helpText)
	{
		Dm.ShowDialogue(FactManager.Instance.TopicsById["you"], helpText, () => { });
		Dm.ShowsHelpText = true;
	}
}
