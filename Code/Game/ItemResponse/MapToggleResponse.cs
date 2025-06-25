/// <summary>
/// respond and switch geometry on/off
/// </summary>
[Group("Brozenye/ItemResponse")]
public class MapToggleResponse : Component, IItemResponse {
	[Property, TextArea] public string[] Message { get; set; }
	[Property] public GameObject MeshToToggle { get; set; }
	[Property, Group("Switch"), HideIf("ToggleInstead", true)] public bool SwitchTo { get; set; } = true;
	[Property, Group("Switch")] public bool ToggleInstead { get; set; }

	public void Respond(string name, Dictionary<string, int> alchemy, DialogueNPC npc) {
		npc.YapDialogue(Message);

		if (ToggleInstead) {
			MeshToToggle.Enabled = !MeshToToggle.Enabled;
		} else {
			MeshToToggle.Enabled = SwitchTo;
		}
	}
}
