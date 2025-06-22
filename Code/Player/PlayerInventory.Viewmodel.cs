public partial class PlayerInventory {
	[Property, Feature("Viewmodel")] public ModelRenderer Viewmodel { get; set; }
	[Property, Feature("Viewmodel")] public Vector3 TargetPosition { get; set; }
	[Property, Feature("Viewmodel")] public GameObject ViewmodelParent { get; set; }

	[Property, Feature("Viewmodel")] public Dictionary<string, Texture> PotionEffects { get; set; } = new();

	private int lastItemHash;

	private void updateViewmodelPosition() {
		var wishPosition = TargetPosition * ViewmodelParent.WorldRotation;

		var newPos = ViewmodelParent.WorldPosition + wishPosition;
		Viewmodel.WorldPosition = Viewmodel.WorldPosition.LerpTo(newPos, 0.05f);
	}

	private void updateViewmodel() {
		if (Viewmodel == null) return;

		var hash = SelectedItem == null ? 0 : SelectedItem.GetHashCode();

		if (lastItemHash != hash) {
			if (SelectedItem != null) {
				Viewmodel.Model = SelectedItem.Model.Model;
				Viewmodel.Tint = SelectedItem.Model.Tint;
				Viewmodel.Enabled = true;

				Viewmodel.LocalScale = SelectedItem.LocalScale;
			} else {
				Viewmodel.Enabled = false;
			}

		}

		Log.Info(hash);
		updateViewmodelPosition();
		

		lastItemHash = hash;
	}

}
