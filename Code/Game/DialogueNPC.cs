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
	[Property, Group("Dialogue"), Range(0, 1)] public float JawOpenStrength { get; set; } = 1;
	[Property, Group("Dialogue"), Range(1, 5)] public float JawOpenDecay { get; set; } = 1;

	[Property, Group("Callback")] public Action OnDialogueFinish { get; set; }

	[Property, TextArea, ReadOnly] public string Output { get; private set; }

	private float jawOpen = 0;

	public PlayerWalker Player { get; set; }
	protected override void OnStart() {
		Player = Scene.GetComponentInChildren<PlayerWalker>();
	}

	public void Interact(GameObject source) {
		if (IsYapping) return;

		YapDialogue(Dialogue);
	}

	public async void YapDialogue(string[] dialogueToRead) {
		if (IsYapping) return;
		if (!IsYapping) IsYapping = true;

		foreach (var text in dialogueToRead) {
			Output = "";

			foreach (char letter in text) {
				Output += letter;

				if (LetterSound != null && letter != ' ') {
					Sound.Play(LetterSound, GameObject.WorldPosition);
					jawOpen = JawOpenStrength;
				}

				await Task.DelaySeconds(LetterInterval);
			}

			await Task.DelaySeconds(MessageInterval);
		}

		Output = "";
		IsYapping = false;

		if (OnDialogueFinish != null) {
			OnDialogueFinish();
		}
	}

	protected override void OnUpdate() {
		if (!IsYapping) {
			CitizenAnimation.LookAt = GameObject;
		} else {
			CitizenAnimation.LookAt = Player.GameObject;
		}

		CitizenAnimation.Target.Morphs.Set("openjawL", jawOpen);
		CitizenAnimation.Target.Morphs.Set("openjawR", jawOpen);
		jawOpen -= Time.Delta * JawOpenDecay;

		jawOpen = Math.Clamp(jawOpen, 0, 1);
	}
}
