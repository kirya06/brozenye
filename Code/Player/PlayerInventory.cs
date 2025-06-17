[Group("Brozenye")]
public class PlayerInventory : Component {
	[Property] public int Capacity = 9;
	[Property, ReadOnly] public ItemComponent[] Items { get; private set; }


	[Property, Group("Cursor"), ReadOnly] public int Cursor { get; private set; } = 0;
	[Property, Group("Cursor"), ReadOnly]
	public ItemComponent SelectedItem {
		get {
			if (Cursor > Items.Length - 1) return null;
			return Items[Cursor];
		}
	}
	[Property, Group("Cursor"), Range(0, 1000, 1)] public float ThrowStrength { get; set; } = 1f;


	[Property, Feature("Viewmodel")] public ModelRenderer Viewmodel { get; set; }
	[Property, Feature("Viewmodel")] public Vector3 TargetPosition { get; set; }
	[Property, Feature("Viewmodel")] public GameObject ViewmodelParent { get; set; }


	protected override void OnStart() {
		Items = new ItemComponent[Capacity];
	}


	protected override void OnUpdate() {
		var wheel = Input.MouseWheel;
		if (wheel.Length > 0) {
			Cursor = (Cursor + wheel.y.CeilToInt()).Clamp(0, Capacity - 1);
		}

		if (Input.Pressed("DropItem")) {
			TryDrop();
		}

		if (Input.Pressed("Attack1") && SelectedItem != null) {
			SelectedItem.Use();
		}

		Cursor = handleSlotKeybinds();

		if (Viewmodel != null) {
			if (SelectedItem != null) {
				Viewmodel.Model = SelectedItem.Model.Model;
				Viewmodel.Tint = SelectedItem.Model.Tint;
				Viewmodel.Enabled = true;

				Viewmodel.LocalScale = SelectedItem.LocalScale;
			} else {
				Viewmodel.Enabled = false;
			}

			updateViewmodelPosition();
		}
		
	}

	private int handleSlotKeybinds() {
		if (Input.Pressed("Slot1")) return 0;
		if (Input.Pressed("Slot2")) return 1;
		if (Input.Pressed("Slot3")) return 2;
		if (Input.Pressed("Slot4")) return 3;
		if (Input.Pressed("Slot5")) return 4;
		if (Input.Pressed("Slot6")) return 5;
		if (Input.Pressed("Slot7")) return 6;
		if (Input.Pressed("Slot8")) return 7;
		if (Input.Pressed("Slot9")) return 8;

		return Cursor;
	}

	private void updateViewmodelPosition() {
		var wishPosition = TargetPosition * ViewmodelParent.WorldRotation;

		var newPos = ViewmodelParent.WorldPosition + wishPosition;
		Viewmodel.WorldPosition = Viewmodel.WorldPosition.LerpTo(newPos, 0.05f);
	}

	public void TryDrop() {
		if (SelectedItem is null) return;
		var cam = Scene.Camera;

		SelectedItem.RemoveFromInventory(cam.WorldPosition);
		SelectedItem.Punch(cam.WorldRotation.Forward * ThrowStrength);
		Items[Cursor] = null;
	}

	public bool IsFull() {
		foreach (var item in Items) {
			if (item is null) return false;
		}

		return true;
	}
}
