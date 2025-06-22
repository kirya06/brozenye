using System.Drawing;

/// <summary>
/// Holds alchemic properties and converts items into potions.
/// </summary>
[Group("Brozenye")]
public class CauldronComponent : Component, IInteractable {

	[Property, RequireComponent] public Collider Collider { get; set; }
	[Property, ReadOnly] public Dictionary<string, int> AlchemicProperties { get; set; } = new();
	[Property, ReadOnly] public Color BrewColor { get; set; } = Color.White;
	[Property, Group("Dependency")] public CauldronList Panel { get; set; }
	[Property, Group("Dependency")] public ModelRenderer Brew { get; set; }

	private SoundEvent soundHotWater = new SoundEvent("sounds/hot-water.sound"); 
	private SoundEvent soundPourWater = new SoundEvent("sounds/water-pour.sound"); 

	protected override void OnStart() {
		Collider.OnObjectTriggerEnter += onTriggerEnter;
	}

	private void onTriggerEnter(GameObject obj) {
		if (obj.Tags.Has("potion")) return;
		var item = obj.GetComponent<ItemComponent>();
		if (item is null) return;

		foreach (var keyval in item.AlchemicProperties) {
			if (AlchemicProperties.ContainsKey(keyval.Key)) {
				AlchemicProperties[keyval.Key] += keyval.Value;
				continue;
			}

			// reset the whole ingredient list
			if (keyval.Key == "Reset") {
				AlchemicProperties = new();
				break;
			}

			AlchemicProperties.Add(keyval.Key, keyval.Value);
		}

		BrewColor = BrewColor.LerpTo(item.BrewColor, 0.5f);

		item.Enabled = false;
		item.GameObject.Destroy();

		Sound.Play(soundHotWater, WorldPosition);
	}

	public void Interact(GameObject source) {
		var inventory = source.GetComponent<PlayerInventory>();
		if (inventory is null) return;
		if (inventory.IsFull()) return;

		var newPot = GameObject.GetPrefab("prefabs/items/potion.prefab").Clone();
		newPot.Parent = Scene;

		var item = newPot.GetComponent<ItemComponent>();

		foreach (var props in AlchemicProperties) {
			item.AlchemicProperties.Add(props.Key, props.Value);
		}

		newPot.GetComponent<ModelRenderer>().Tint = BrewColor;

		item.Interact(source);
		Sound.Play(soundPourWater, WorldPosition);

	}

	protected override void OnUpdate() {
		if (Panel != null)
			Panel.Properties = AlchemicProperties;

		Brew.Tint = BrewColor;
	}

}
