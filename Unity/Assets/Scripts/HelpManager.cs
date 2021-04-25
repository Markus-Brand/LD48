using UnityEngine;

public class HelpManager : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H) && !DialogueManager.GetInstance().IsCurrentlyActive) {
			var discoverableFacts = FactManager.Instance.AllDiscoverableFacts;
			if (discoverableFacts.Count == 0) {
				ShowHelp("There is nothing more to do, you won the game!");
			} else {
				ShowHelp(discoverableFacts[0].HelpText);
			}
		}
	}

	private void ShowHelp(string helpText)
	{
		DialogueManager.GetInstance().ShowDialogue(Speaker.You, helpText, () => { });
	}
}
