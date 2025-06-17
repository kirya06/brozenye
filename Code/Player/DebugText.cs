/// <summary>
/// Draws debug text to the screen. Temporary hud replacement.
/// </summary>
[Group("Brozenye")]
public class DebugText : Component {
	[Property, RequireComponent] public PlayerInventory Inventory { get; set; }

	protected override void OnUpdate() {
		var item = Inventory.SelectedItem != null ? Inventory.SelectedItem.ToString() : "None";
		var text = $"""
		Slot #{Inventory.Cursor + 1}
		{item}
		""";
		DebugOverlay.ScreenText(Vector2.Zero, text, 14, TextFlag.LeftTop);
	}

}
