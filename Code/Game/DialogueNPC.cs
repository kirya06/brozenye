using Sandbox.Citizen;

[Group("Brozenye")]
public class DialogueNPC : Component, IInteractable {
	[Property, Group("Dependency")] public CitizenAnimationHelper CitizenAnimation { get; set; }
	[Property, Group("Dependency")] public WorldPanel Panel { get; set; }
	[Property, Group("Dependency")] public BoxCollider Collider { get; set; }

	[Property, Group("Dialogue"), TextArea] public string[] Dialogue { get; set; }
	[Property, Group("Dialogue"), Range(0, 2.5f)] public float LetterInterval { get; set; } = 0.05f;
	[Property, Group("Dialogue"), Range(0, 5, 0.1f)] public float MessageInterval { get; set; } = 1f;
	[Property, Group("Dialogue"), ReadOnly] public bool IsYapping { get; private set; } = false;
	[Property, Group("Dialogue")] public SoundEvent LetterSound { get; set; }

	[Property, TextArea, ReadOnly] public string Output { get; private set; }

	[Property, Feature("Give Item")] public bool EnableRecievingItems { get; set; } = false;
	[Property, Feature("Give Item"), HideIf("EnableRecievingItems", false)] public Func<bool> RecieveCondition { get; set; }

	public PlayerWalker Player { get; set; }
	protected override void OnStart() {
		Player = Scene.GetComponentInChildren<PlayerWalker>();
		Collider.OnObjectTriggerEnter += triggerEnterItem;
	}

	public void Interact(GameObject source) {
		if (IsYapping) return;

		IsYapping = true;
		YapDialogue(Dialogue);
	}

	public async void YapDialogue(string[] dialogueToRead) {
		foreach (var text in dialogueToRead) {
			Output = "";

			foreach (char letter in text) {
				Output += letter;

				if (LetterSound != null) {
					Sound.Play(LetterSound, GameObject.WorldPosition);
				}

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

	private void triggerEnterItem(GameObject obj) {
		if (!obj.Tags.Has("item")) return;

		if (EnableRecievingItems) {
			if (RecieveCondition is null) return;
			bool recieved = RecieveCondition.Invoke();

			if (recieved) {
				obj.Destroy();
			}
		}
	}
}
