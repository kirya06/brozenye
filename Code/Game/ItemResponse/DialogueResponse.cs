[Group("Brozenye/ItemResponse")]
public class DialogueResponse : Component, IItemResponse {
	[Property, TextArea] public string[] Message { get; set; }

	public void Respond(string name, Dictionary<string, int> alchemy, DialogueNPC npc) {
		npc.YapDialogue(Message);
	}
}
