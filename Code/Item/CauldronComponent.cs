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
	[Property, Group("Particles")] public Dictionary<string, GameObject> PropertyParticles { get; set; }

	private SoundEvent soundHotWater = new SoundEvent("sounds/hot-water.sound");
	private SoundEvent soundPourWater = new SoundEvent("sounds/water-pour.sound");
	private GameObject particleParent;

	protected override void OnStart() {
		Collider.OnObjectTriggerEnter += onTriggerEnter;

		particleParent = new GameObject();
		particleParent.Name = "Particles";
		particleParent.SetParent(GameObject, false);
		particleParent.LocalScale = new Vector3(0.5f, 0.5f, 0.5f);
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

			if (keyval.Key == "Reverse") {
				foreach (var prop in AlchemicProperties) {
					AlchemicProperties[prop.Key] = -prop.Value;
				}
				break;
			}

			AlchemicProperties.Add(keyval.Key, keyval.Value);
		}

		RebuildParticles();

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

	public void RebuildParticles() {

		foreach (var obj in particleParent.Children) {
			obj.Destroy();
		}

		foreach (var keyval in AlchemicProperties) {
			if (!PropertyParticles.ContainsKey(keyval.Key)) continue;

			var particle = PropertyParticles[keyval.Key].Clone();
			particle.Name = keyval.Key;
			particle.SetParent(particleParent, false);

			SetBrewPartice(particle, keyval.Value);
		}
	}

	public static void SetBrewPartice(GameObject obj, int propertyIntensity) {
		var particles = obj.GetComponent<ParticleEffect>();
		particles.Tint = Color.Red.LerpTo(Color.Green, (float)propertyIntensity / 3f);

		var newScale = new ParticleFloat();
		newScale.ConstantValue = Math.Abs(propertyIntensity * 2f);
		particles.Scale = newScale;

		if (propertyIntensity == 0) particles.Enabled = false;
	}

}
