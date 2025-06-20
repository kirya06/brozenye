public class RubleReward : Component, IItemResponse {
	[Property, TextArea] public string[] Message { get; set; }
	[Property] public int Reward { get; set; }
	[Property] public bool DoItOnce { get; set; }
	public void Respond(string itemName, Dictionary<string, int> alchemy, DialogueNPC npc = null) {
		npc.YapDialogue(Message);

		Scene.GetComponentInChildren<PlayerInventory>().Rubles += Reward;

		if (DoItOnce) {
			var reciever = GameObject.GetComponent<ItemReciever>();
			reciever.ItemWhitelist.Remove(itemName);
		}
	}
}
