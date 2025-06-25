public class RubleReward : Component, IItemResponse {
	[Property, TextArea] public string[] Message { get; set; }
	[Property] public int Reward { get; set; }
	[Property] public bool DoItOnce { get; set; }

	/// <summary>
	/// When giving an item you rewrite the base NPC dialogue (the one that comes out on interact)
	/// </summary>
	[Property, Feature("Rewrite")] public bool EnableRewrite { get; set; }
	[Property, Feature("Rewrite")] public bool UseRewardResponse { get; set; } = true;
	[Property, Feature("Rewrite"), HideIf("UseRewardResponse", true)] public string[] RewrittenMessage { get; set; }

	private SoundEvent rewardSound = new SoundEvent("sounds/handling_coins.sound");

	public void Respond(string itemName, Dictionary<string, int> alchemy, DialogueNPC npc = null) {
		npc.YapDialogue(Message);

		Scene.GetComponentInChildren<PlayerInventory>().Rubles += Reward;

		if (DoItOnce) {
			var reciever = GameObject.GetComponent<ItemReciever>();
			reciever.ItemWhitelist.Remove(itemName);
		}

		if (EnableRewrite) {
			npc.Dialogue = UseRewardResponse ? Message : RewrittenMessage;
		}

		Sound.Play(rewardSound, WorldPosition);
	}
}
