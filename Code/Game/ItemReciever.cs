[Group("Brozenye")]
public class ItemReciever : Component {
	[Property, HideIf("UseBlacklist", true)] public List<string> ItemWhitelist { get; set; } = new();
	[Property, HideIf("UseBlacklist", false)] public List<string> ItemBlacklist { get; set; } = new();
	[Property] public bool UseBlacklist { get; set; }

	[Property] public IItemResponse Response { get; set; }

	[Property, Group("Dependency")] public BoxCollider Collider { get; set; }
	[Property, Group("Dependency"), HideIf("EnableNPC", false)] public DialogueNPC NPC { get; set; }
	[Property, Group("Dependency")] public bool EnableNPC { get; set; }

	protected override void OnStart() {
		Collider.OnObjectTriggerEnter += onTrigger;

		Response = GetComponent<IItemResponse>();
	}

	private void onTrigger(GameObject obj) {
		if (!obj.Tags.Has("item")) return;
		if (EnableNPC) {
			if (NPC.IsYapping) return;
		}

		var item = obj.GetComponent<ItemComponent>();
		var contains = UseBlacklist ? !ItemBlacklist.Contains(item.Name) : ItemWhitelist.Contains(item.Name);
		if (contains) {
			if (Response != null) Response.Respond(item.Name, item.AlchemicProperties, NPC);
			obj.Destroy();
		}
	}
}
