public partial class PlayerInventory {
	[Property, Feature("Viewmodel")] public ModelRenderer Viewmodel { get; set; }
	[Property, Feature("Viewmodel")] public Vector3 TargetPosition { get; set; }
	[Property, Feature("Viewmodel")] public GameObject ViewmodelParent { get; set; }

	[Property, Feature("Viewmodel")] public Dictionary<string, GameObject> PotionEffects { get; set; } = new();

	private int lastItemHash;
	private List<GameObject> particleObj = new();

	private void updateViewmodelPosition() {
		var wishPosition = TargetPosition * ViewmodelParent.WorldRotation;

		var newPos = ViewmodelParent.WorldPosition + wishPosition;
		Viewmodel.WorldPosition = Viewmodel.WorldPosition.LerpTo(newPos, 0.05f);
	}

	private void updateViewmodel() {
		if (Viewmodel == null) return;

		var hash = SelectedItem == null ? 0 : SelectedItem.GetHashCode();

		if (lastItemHash != hash) {

			foreach (var obj in particleObj) {
				obj.Destroy();
			}
			particleObj = new();

			if (SelectedItem != null) {
				Viewmodel.Model = SelectedItem.Model.Model;
				Viewmodel.Tint = SelectedItem.Model.Tint;
				Viewmodel.Enabled = true;

				Viewmodel.LocalScale = SelectedItem.LocalScale;

				if (SelectedItem.Tags.Has("potion")) {
					setParticles();
				}
			} else {
				Viewmodel.Enabled = false;
			}

		}

		updateViewmodelPosition();
		lastItemHash = hash;
	}

	private void setParticles() {
		foreach (var keyval in SelectedItem.AlchemicProperties) {
			if (PotionEffects.ContainsKey(keyval.Key)) {
				var newParticle = PotionEffects[keyval.Key].Clone();
				newParticle.SetParent(Viewmodel.GameObject, false);

				CauldronComponent.SetBrewPartice(newParticle, keyval.Value);

				particleObj.Add(newParticle);
			}
		}
	}

}
