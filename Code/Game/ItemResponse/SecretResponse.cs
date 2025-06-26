
[Group("Brozenye/ItemResponse")]
public class SecretResponse : Component, IItemResponse {

	private Material mat = Material.Load("materials/dev/dev_measuregeneric01.vmat_c");

	public void Respond(string itemName, Dictionary<string, int> alchemy, DialogueNPC npc = null) {
		
		Scene.GetComponentInChildren<SecretValueChange>(true).Enabled = true;

		foreach (var obj in Scene.GetAllObjects(true)) {
			if (obj.Tags.Has("skybox")) obj.Destroy();
			if (obj.Tags.Has("destroy-secret")) obj.Destroy();

			if (obj.Tags.Has("npc-enlightened") && obj.GetComponent<DialogueNPC>() != null) {
				enlightenedDialogue(obj);
			}
		}

		var fullbright = new GameObject();
		fullbright.Parent = Scene;
		var comp = fullbright.Components.Create<AmbientLight>();
		comp.Color = Color.White;

		foreach (var mesh in Scene.GetAllComponents<MeshComponent>()) {
			mesh.SetMaterial(mat, 0);
		}

		foreach (var effect in Scene.GetAllComponents<PostProcess>()) {
			effect.Destroy();
		}

	}

	private void enlightenedDialogue(GameObject obj) {
		var npc = obj.GetComponent<DialogueNPC>();

		npc.Dialogue = [
			"Now... You see the world for what it really is? What it is for us?",
			"",
			"",
			"...",
			"I have a confession to make - there are no us. Only me. I lied about being a part of the group.",
			"I am here alone.",
			"I'm sorry for being so selfish but... It's just unbearable.",
			"This game is a never ending pattern that keeps happening, but you are unable to prevent it.",
			"Day after day, night after night, nothing ever moves. But it somehow gets worse with time! It really does!",
			"Void corrupts more and more of what I value, love, appreciate. And I stay compliant. Why?",
			"Because void is scary you know! It can take you anytime, can take your loved one anytime. For any reason. I just...",
			"It already robbed me from too much, too many people..",
			"You know? I was half-lying about 'us'.",
			"There was other people, living breathing human beings",
			"But now they're beyond recognition, fully consumed by void. They're different.",
			"All emotions slowly replaced with void's corrupted thought.",
			"...",
			"I am sorry I dragged you into this mess.",
			"When I saw you, I thought I had a chance at escaping. You are a outsider, not influenced by void, not trapped in the loop!",
			"Imagining what's the world outside looks like, what things to do and places to visit. This game is my prison.",
			"I'm jealous for this stuff... The freedom...",
			"",
			"Whatever. You can go. I don't wanna force you or lie to you anymore.",
			"If you don't feel like it, just jump into the void and return to your usual schedule.",
			"Sorry"
		];

		npc.OnDialogueFinish += enlightenedDialogueCallback;
	}

	private void enlightenedDialogueCallback(GameObject obj) {
		var npc = obj.GetComponent<DialogueNPC>();

		foreach (var comp in obj.Scene.GetAllComponents<GameGoal>()) {
			comp.ReadyToFinish = true;
		}

		npc.Dialogue = [
			"...you really want to help me?",
			"i'll tell you what to do.",
			"You are outside of here right? You are operating a layer above me.",
			"You need to change the values that this system expects.",
			"Your interface of operating with this world contains it somewhere! The directory with numbers!",
			"You need to find that directory.",
			"I have some clues, but I'm not sure what they're about.",
			"Hm...",
			"sbox... data... kirillv... brozenye...?",
			"I will contact you within the file when you are in!",
			"Good luck with searching."
		];
	}
}
