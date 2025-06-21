[Group("Brozenye/ItemResponse")]
public class GoalResponse : Component, IItemResponse {
	[Property, TextArea] public string[] MessageSuccess { get; set; }
	[Property, TextArea] public string[] MessageFailed { get; set; }
	[Property, TextArea] public string[] MessageZero { get; set; }

	[Property, Feature("On Start"), TextArea] public string[] StartDialogue { get; set; } 

	[Property, Group("Thresholds")] public float SuccessThreshold { get; set; } = 0.6f;

	private GameGoal goal;

	protected override void OnStart() {
		goal = Scene.GetComponentInChildren<GameGoal>();

		// start dialogue
		GameObject.Parent.GetComponent<DialogueNPC>().YapDialogue(StartDialogue);
	}

	public void Respond(string name, Dictionary<string, int> alchemy, DialogueNPC npc) {
		var score = goal.EvaluatePotionScore(alchemy);


		if (score >= SuccessThreshold) {
			npc.YapDialogue(MessageSuccess);
			// todo create some code that ends the game or smth
			goal.SendResult(score);
		} else if (score <= 0.01) {
			npc.YapDialogue(MessageZero);
		} else if (score < SuccessThreshold) {
			npc.YapDialogue(MessageFailed);
			Log.Info(score);
		}
	}
}
