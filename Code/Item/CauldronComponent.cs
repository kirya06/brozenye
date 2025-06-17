/// <summary>
/// Holds alchemic properties and converts items into potions.
/// </summary>
[Group("Brozenye")]
public class CauldronComponent : Component {

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

}
