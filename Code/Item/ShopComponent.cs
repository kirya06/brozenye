public class ShopComponent : Component, IInteractable {
	[Property] public GameObject Item { get; set; }
	[Property] public int Price { get; set; }
	/// <summary>
	/// Don't draw hud and don't play buy sound when shown
	/// </summary>
	[Property] public bool Secret { get; set; }
	private SoundEvent buySound = new SoundEvent("sounds/cash.sound");

	public void Interact(GameObject source) {
		var inventory = source.GetComponent<PlayerInventory>();
		if (inventory == null) return;

		if (inventory.Rubles > Price && !inventory.IsFull()) {
			inventory.Rubles -= Price;

			var item = Item.Clone();
			item.GetComponent<ItemComponent>().Interact(source);
			
			if (!Secret)
				Sound.Play(buySound, WorldPosition);
		}
	}
}
