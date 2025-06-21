public class ShopComponent : Component, IInteractable {
	[Property] public GameObject Item { get; set; }
	[Property] public int Price { get; set; }
	private SoundEvent buySound = new SoundEvent("sounds/cash.sound");

	public void Interact(GameObject source) {
		var inventory = source.GetComponent<PlayerInventory>();
		if (inventory == null) return;

		if (inventory.Rubles > Price && !inventory.IsFull()) {
			inventory.Rubles -= Price;

			var item = Item.Clone();
			item.GetComponent<ItemComponent>().Interact(source);

			Sound.Play(buySound, WorldPosition);
		}
	}
}
