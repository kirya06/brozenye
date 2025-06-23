[Group("Brozenye")]
public class GameGoal : Component {
	[Property] public GoalResource[] PossibleGoals { get; set; }
	[Property, ReadOnly] public int CurrentGoalIndex { get; private set; } = 0;
	public GoalResource CurrentGoal => PossibleGoals[CurrentGoalIndex];

	[Property, ReadOnly] public bool ReadyToFinish { get; private set; }
	public float LastScore { get; private set; }

	[Property, Group("Spawnpoints")] public GameObject NPCSpawn { get; set; }
	[Property, Group("Spawnpoints")] public GameObject SampleSpawn { get; set; }

	protected override void OnStart() {
		initializeMainQuest();
	}
	/// <summary>
	/// Evaluates a dictionary containing potion properties with chosen goal. 0f - horrible, no match. 1f - ideal, 1:1 match.
	/// </summary>
	public static float EvaluatePotionScore(Dictionary<string, int> original, Dictionary<string, int> replica, float maxAttributePower = 3f) {
		float score = 0f;

		float attributePrice = 1f / (float)original.Count;

		foreach (var keyval in replica) {
			float replicaValue = (float)replica[keyval.Key];

			if (original.ContainsKey(keyval.Key)) {
				float scoreAddition = (replicaValue / (float)original[keyval.Key]) * attributePrice;
				score += scoreAddition;
			} else {
				float scorePenalty = (replicaValue / maxAttributePower) * attributePrice;
				score -= Math.Abs(scorePenalty);
			}
		}

		return Math.Clamp(score, -1, 1);
	}

	public void SendResult(float score) {
		LastScore = score;
		ReadyToFinish = true;
	}

	public void NextGoal() {
		var nextGoal = CurrentGoalIndex + 1;
		if (PossibleGoals.Length - 1 < nextGoal) {
			FileSystem.Data.WriteAllText("last-score.txt", (LastScore * 100).CeilToInt().ToString());
			Scene.LoadFromFile("scenes/results.scene");
		} else {
			CurrentGoalIndex = nextGoal;
			initializeMainQuest();
		}
	}

	private void initializeMainQuest() {
		// delete old npc first, if exists
		foreach (var obj in NPCSpawn.Children) {
			obj.Destroy();
		}

		// clone a dialogue npc from current goal
		var newNPC = CurrentGoal.DialogueNPC.Clone();
		newNPC.SetParent(NPCSpawn, false);

		// create a sample potion
		var sample = GameObject.GetPrefab("prefabs/items/potion-sample.prefab").Clone();
		sample.SetParent(SampleSpawn, false);
		var item = sample.GetComponent<ItemComponent>();

		foreach (var keyval in CurrentGoal.AlchemicProperties) item.AlchemicProperties.Add(keyval.Key, keyval.Value);
		sample.GetComponent<ModelRenderer>().Tint = CurrentGoal.BrewColor;
	}
}
