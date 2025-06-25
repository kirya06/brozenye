public class WinTrigger : Component {
	[Property, RequireComponent] public Collider Collider { get; set; }

	private GameGoal goal;
	private bool thrownYourself = false;

	protected override void OnStart() {
		Collider.OnObjectTriggerEnter += onTrigger;
		goal = Scene.GetComponentInChildren<GameGoal>();
	}

	private void onTrigger(GameObject obj) {
		handleJumpinOffACliff(obj);

		if (obj.Tags.Has("player") && goal.ReadyToFinish) {
			goal.NextGoal();
		}
	}

	private void handleJumpinOffACliff(GameObject obj) {
		if (obj.Tags.Has("player") && !thrownYourself) {
			var guy = Scene.GetAllObjects(true).FirstOrDefault(x => x.Name == "pit-liker");
			guy.GetComponent<DialogueNPC>().Dialogue = [
				"you actually just jumped??",
				"",
				"how does that feel?",
				"when i threw a loaf of bread, it turned out hard as rock and stale when it came back",
				"are you hard as rock and stale?"
			];
			thrownYourself = true;
		}

		if (obj.Tags.Has("item")) {
			var item = obj.GetComponent<ItemComponent>();
			if (item.Name == "Bread") {
				var newItem = GameObject.GetPrefab("prefabs/items/stale-bread.prefab").Clone();
				newItem.Parent = Scene;
				newItem.WorldPosition = new Vector3(0, 0, 4500);

				obj.Destroy();
				return;
			}

			if (item.Name == "Snow") {
				var newItem = GameObject.GetPrefab("prefabs/items/anti-snow.prefab").Clone();
				newItem.Parent = Scene;
				newItem.WorldPosition = new Vector3(0, 0, 4500);

				obj.Destroy();
				return;
			}
		}

		obj.WorldPosition = new Vector3(0, 0, 4500);
	}
}
