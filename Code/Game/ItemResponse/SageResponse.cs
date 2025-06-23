public class SageResponse : Component, IItemResponse {
	[Property, TextArea] public Dictionary<string, string> NameToDialogue { get; set; } = new();
	public void Respond(string name, Dictionary<string, int> alchemy, DialogueNPC npc) {
		if (NameToDialogue.ContainsKey(name)) {
			npc.YapDialogue([NameToDialogue[name]]);
		}
	}
}
