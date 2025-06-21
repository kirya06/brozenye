[Group("Brozenye")]
public class GameGoal : Component {
	[Property] public GoalResource[] PossibleGoals { get; set; }
	[Property, ReadOnly] public GoalResource ChosenGoal { get; private set; }
	
	[Property, ReadOnly] public bool ReadyToFinish { get; private set; }
	public float LastScore { get; private set; }

	protected override void OnStart() {
		ChosenGoal = Game.Random.FromArray(PossibleGoals);
	}

	/// <summary>
	/// Evaluates a dictionary containing potion properties with chosen goal. 0 - horrible, no match. 10 - ideal, 1:1 match.
	/// </summary>
	public float EvaluatePotionScore(Dictionary<string, int> potionProperties) {
		float score = 0;
		float scorePerAttribute = 1 / (float)ChosenGoal.AlchemicProperties.Count;
		foreach (var keyval in potionProperties) {
			if (ChosenGoal.AlchemicProperties.ContainsKey(keyval.Key)) {
				var stat = (float)keyval.Value / (float)ChosenGoal.AlchemicProperties[keyval.Key];
				score += stat * scorePerAttribute;
			} else {
				if (keyval.Value < 0) score += (scorePerAttribute / 4) * (float)keyval.Value;
			}
		}

		return score;
	}

	public void SendResult(float score) {
		LastScore = score;
		ReadyToFinish = true;
	}
}
