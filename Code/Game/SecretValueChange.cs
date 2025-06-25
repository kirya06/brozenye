public class SecretValueChange : Component {

	protected override void OnFixedUpdate() {
		var file = getScore();

		if (file < 0) {
			Scene.LoadFromFile("scenes/outside.scene");
		}
	}


	private int getScore() {
		if (FileSystem.Data.FileExists("last-score.txt")) {
			return FileSystem.Data.ReadAllText("last-score.txt").ToInt();
		}

		return 0;
	}
}
