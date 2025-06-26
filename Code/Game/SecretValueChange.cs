using System.Buffers.Text;
using System.Text;

public class SecretValueChange : Component {

	public static readonly float TIME_DELAY = 1;
	public static readonly string SECRET_FILENAME = "DO-NOT-CHANGE_profile.json";

	float tick = Time.Now;
	protected override void OnFixedUpdate() {
		if (Time.Now - tick < TIME_DELAY) return;

		var secretActive = IsSecret();

		if (secretActive) {
			Scene.LoadFromFile("scenes/outside.scene");
		}

		tick = Time.Now;
	}


	public static bool IsSecret() {
		var data = FileSystem.Data.ReadJson<SecretJson>(SECRET_FILENAME);

		if (data == null) {
			var json = new SecretJson();

			json.Name = "Hleb";
			var bytes = Encoding.UTF8.GetBytes(
				"Hey! You found the place! Now's the most important part - you need to change the DATA variable. It should be something very precise, random number wouldn't help. I have found a weakness within loop, the number that theoretically will end the its existence. I don't have a number directly but I have a good clue what it can be. So... It's a year. And the words that describing it are 'February', 'Rebellion', 'People', 'Red'. You have any clues?"
			);
			json.OUTSIDER_README_THIS_IS_ME = Convert.ToBase64String(bytes);
			json.DATA = 12367;

			FileSystem.Data.WriteJson<SecretJson>(SECRET_FILENAME, json);

			data = json;
		}

		return data.DATA == 1917;
	}

	public class SecretJson {
		public string Name { get; set; }
		public string OUTSIDER_README_THIS_IS_ME { get; set; }
		public int DATA { get; set; }
	}
}
