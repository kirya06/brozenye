public class WinTrigger : Component {
	[Property, RequireComponent] public Collider Collider { get; set; }

	private GameGoal goal;

	protected override void OnStart() {
		Collider.OnObjectTriggerEnter += onTrigger;
		goal = Scene.GetComponentInChildren<GameGoal>();
	}

	private void onTrigger(GameObject obj) {
		Log.Info(obj.Tags.Has("player"));
		if (!obj.Tags.Has("player")) return;
		if (!goal.ReadyToFinish) {
			obj.WorldPosition = new Vector3(0, 0, 3500);
			return;
		}

		FileSystem.Data.WriteAllText("last-score.txt", (goal.LastScore * 100).CeilToInt().ToString());
		Scene.LoadFromFile("scenes/results.scene");
	}
}
