/// <summary>
/// Items that can be picked up by player. Also has special properties thats used in alchemy proccess
/// </summary>
[Group("Brozenye")]
public class ItemComponent : Component, IInteractable {
	[Property] public string Name { get; set; } = "???";
	/// <summary>
	/// Is currently in inventory? (turns off all physics/render stuff)
	/// </summary>
	[Property, ReadOnly] public bool InInventory { get; private set; } = false;

	[Property, Group("Active")] public Action OnUse { get; set; } = () => {};

	[Property, Group("Alchemy")] public Dictionary<string, int> AlchemicProperties { get; set; } = new();

	[Property, Group("Dependency"), RequireComponent] public Rigidbody Rigidbody { get; set; }
	[Property, Group("Dependency"), RequireComponent] public ModelRenderer Model { get; set; }
	[Property, Group("Dependency"), RequireComponent] public HighlightOutline Outline { get; set; }

	public static readonly float HOVER_TIME_LENGTH = 0.1f;
	float hoverTime = new();


	public void Interact(GameObject source) {
		if (!source.Tags.Has("player")) return;
		var inventory = source.GetComponent<PlayerInventory>();

		if (InInventory) return;
		//	if hand is busy, try to fill next slot with a item
		var slotToFill = inventory.Cursor;
		if (inventory.SelectedItem != null) {

			for (int i = inventory.Cursor; i < inventory.Items.Length; i++) {
				if (inventory.Items[i] == null) {
					slotToFill = i;
					break;
				}
			}

		}

		toggleVisibility(false);
		InInventory = true;
		inventory.Items[slotToFill] = this;
	}

	public void Hover(bool hover) {
		Outline.Enabled = true;
		hoverTime = Time.Now;
	}

	public void RemoveFromInventory(Vector3 newPos) {
		toggleVisibility();
		InInventory = false;

		WorldPosition = newPos;
	}

	private void toggleVisibility(bool visible = true) {
		Rigidbody.Enabled = visible;
		Model.Enabled = visible;
	}

	public void Punch(Vector3 velocity) => Rigidbody.ApplyImpulse(velocity * Rigidbody.Mass);

	public void Use() => OnUse.Invoke();

	protected override void OnUpdate() {
		if (Time.Now - hoverTime > HOVER_TIME_LENGTH) {
			Outline.Enabled = false;
		}
	}

	public override string ToString() {
		return Name.ToUpper();
	}
}
