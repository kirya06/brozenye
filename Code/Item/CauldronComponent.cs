/// <summary>
/// Holds alchemic properties and converts items into potions.
/// </summary>
[Group("Brozenye")]
public class CauldronComponent : Component, IInteractable {

	[Property, RequireComponent] public Collider Collider { get; set; }
	[Property, ReadOnly] public Dictionary<string, int> AlchemicProperties { get; set; }

	protected override void OnStart() {
		Collider.OnObjectTriggerEnter += onTriggerEnter;
	}

	private void onTriggerEnter(GameObject obj) {
		var item = obj.GetComponent<ItemComponent>();
		if (item is null) return;

		foreach (var keyval in item.AlchemicProperties) {
			if (AlchemicProperties.ContainsKey(keyval.Key)) {
				AlchemicProperties[keyval.Key] += keyval.Value;
				continue;
			}

			AlchemicProperties.Add(keyval.Key, keyval.Value);
		}

		item.Enabled = false;
		item.GameObject.Destroy();
	}

	public void Interact(GameObject source) {
		Log.Info("yeh uh");
		var inventory = source.GetComponent<PlayerInventory>();
		if (inventory is null) return;
		if (inventory.IsFull()) return;

		var newPot = GameObject.GetPrefab("prefabs/items/potion.prefab").Clone();
		newPot.Parent = Scene;
		newPot.GetComponent<ItemComponent>().Interact(source);
	} 

}
