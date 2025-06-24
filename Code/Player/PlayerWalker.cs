/// <summary>
/// Simple walk player controller.
/// </summary>
[Group("Brozenye")]
public partial class PlayerWalker : Component {
	[Property, RequireComponent, HideIf("HideComponent", true), Group("CharacterController")] public CharacterController Controller { get; set; }
	[Property, Group("CharacterController")] public bool HideComponent { get; set; } = true;

	[Property, Group("Stats"), Range(0, 1000, 1)] public float WalkSpeed { get; set; } = 250;
	[Property, Group("Stats"), Range(0, 1000, 1)] public float JumpPower { get; set; } = 250;
	[Property, Group("Stats"), Range(0, 10, 0.01f)] public float Friction { get; set; } = 1f;
	[Property, Group("Stats"), Range(0, 10)] public float AirFriction { get; set; } = 1f;
	[Property, Group("Stats"), Range(1, 5)] public float SprintMultiplier { get; set; } = 2f;
	[Property, Group("Stats")] public SoundEvent Footsteps { get; set; }
	[Property, Group("Stats"), Range(0, 5)] public float FootstepInterval { get; set; } = 0.15f;
	private float lastFootstep = Time.Now;


	[Property, Feature("Debug")] public Vector3 WishDirection => Input.AnalogMove.ClampLength(1);
	[Property, Feature("Debug")] public Angles WishLook => Input.AnalogLook;
	[Property, Feature("Debug")] public Vector3 Gravity => Scene.PhysicsWorld.Gravity;
	[Property, Feature("Debug")] public CameraComponent Camera { get; private set; }
	[Property, Feature("Debug")] public float MovementMultiplier { get; set; } = 1;
	

	protected override void OnStart() {
		Camera = GameObject.GetComponentInChildren<CameraComponent>();
	}

	protected override void OnFixedUpdate() {
		var velocity = WishDirection * WalkSpeed;

		MovementMultiplier = updateMovementModifier();

		velocity *= MovementMultiplier;
		velocity *= LocalRotation;

		if (Controller.IsOnGround) {
			Controller.Velocity = Controller.Velocity.WithZ(0);

			Controller.Accelerate(velocity);
			Controller.ApplyFriction(Friction);
			if (velocity.Length > 0) updateFootstepSounds();
		} else {
			Controller.Velocity += Gravity * Time.Delta * 0.5f;
			Controller.Accelerate(WishDirection);
			Controller.ApplyFriction(AirFriction);
		}

		if (Input.Pressed("Jump") && Controller.IsOnGround) Controller.Punch(Vector3.Up * JumpPower * MovementMultiplier);

		Controller.Move();
	}

	private float updateMovementModifier() {
		float modifier = 1;

		if (Input.Down("Duck")) return modifier / 2;
		if (Input.Down("Run")) modifier *= SprintMultiplier;

		return modifier;
	}

	private void updateFootstepSounds() {
		//Log.Info(Time.Now - lastFootstep > FootstepInterval);

		var modifyBy = (MovementMultiplier - 1.25) * FootstepInterval;
		if (Time.Now - lastFootstep > FootstepInterval - modifyBy) {
			lastFootstep = Time.Now;
			Sound.Play(Footsteps, WorldPosition + (WishDirection * LocalRotation) * 35);
		}
	}

	protected override void OnUpdate() {
		updateInteraction();

		if (Camera is null) return;

		updateCamera();
	}

	protected override void DrawGizmos() {
		Gizmo.Draw.SolidBox(Controller.BoundingBox);
	}
}
