using UnityEngine;

public class HelpManager : MonoBehaviour
{
	private static DialogueManager Dm => DialogueManager.GetInstance();
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H)) {
			if (!Dm.IsCurrentlyActive) {
				var discoverableFacts = FactManager.Instance.AllDiscoverableFacts;
				if (discoverableFacts.Count == 0) {
					ShowHelp("There is nothing more to do, you won the game!");
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
		Dm.ShowDialogue(Speaker.You, helpText, () => { });
		Dm.ShowsHelpText = true;
	}
}
