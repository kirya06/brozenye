[GameResource("Game Goal", "goal", "Describes game's goal for the player")]
public class GoalResource : GameResource {
	public string Title { get; set; }
	public Dictionary<string, int> AlchemicProperties { get; set; }
}
