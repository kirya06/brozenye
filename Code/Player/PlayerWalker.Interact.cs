public partial class PlayerWalker {
	[Property, Feature("Interact")] public float InteractDistance { get; set; } = 25f;
	[Property, Feature("Interact"), ReadOnly] public IInteractable Selected { get; set; }

	private void updateInteraction() {
		var cam = Scene.Camera;
		var from = cam.WorldPosition;
		var to = cam.WorldPosition + cam.WorldRotation.Forward * InteractDistance;

		var trace = Scene.Trace.Ray(from, to)
			.WithTag("interact")
			.Run();

		if (trace.Hit) {
			var itemComp = trace.GameObject.GetComponent<IInteractable>();
			if (itemComp is null) return;

			Selected = itemComp;
		} else Selected = null;

		DebugOverlay.Line(trace.StartPosition, trace.EndPosition, Color.Red, 1);

		if (Selected != null && Input.Pressed("Use")) {
			Selected.Interact(GameObject);
		}
	}

}
