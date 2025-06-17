using Sandbox.Citizen;

[Group("Brozenye")]
public class DialogueNPC : Component, IInteractable {
	[Property, Group("Dependency")] public CitizenAnimationHelper CitizenAnimation { get; set; }
	[Property, Group("Dependency")] public WorldPanel Panel { get; set; }

	[Property, Group("Dialogue"), TextArea] public string[] Dialogue { get; set; }
	[Property, Group("Dialogue"), Range(0, 2.5f)] public float LetterInterval { get; set; } = 0.05f;
	[Property, Group("Dialogue"), Range(0, 5, 0.1f)] public float MessageInterval { get; set; } = 1f;
	[Property, Group("Dialogue"), ReadOnly] public bool IsYapping { get; private set; } = false;

	[Property, TextArea, ReadOnly] public string Output { get; private set; }

	public PlayerWalker Player { get; set; }
	protected override void OnStart() {
		Player = Scene.GetComponentInChildren<PlayerWalker>();
	}

	public void Interact(GameObject source) {
		if (IsYapping) return;

		IsYapping = true;
		YapDialogue();
	}

	public async void YapDialogue() {
		foreach (var text in Dialogue) {
			Output = "";

			foreach (char letter in text) {
				Output += letter;
				await Task.DelaySeconds(LetterInterval);
			}

			await Task.DelaySeconds(MessageInterval);
		}

		Output = "";
		IsYapping = false;
	}

	protected override void OnUpdate() {
		if (!IsYapping) {
			CitizenAnimation.LookAt = GameObject;
		} else {
			CitizenAnimation.LookAt = Player.GameObject;
		}
	}
}
