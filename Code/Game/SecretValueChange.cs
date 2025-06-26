public class SecretValueChange : Component {

	public static readonly float TIME_DELAY = 1;

	float tick = Time.Now;
	protected override void OnFixedUpdate() {
		if (Time.Now - tick < TIME_DELAY) return;

		var file = getScore();

		if (file < 0) {
			Scene.LoadFromFile("scenes/outside.scene");
		}

		Log.Info("secret");
		tick = Time.Now;
	}


	private int getScore() {
		var data = FileSystem.Data.ReadAllText("last-score.txt");
		if (data == null) return 0;

		return data.ToInt();
	}
}
