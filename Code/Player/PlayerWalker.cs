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


	[Property, Group("Camera")] public float CameraHeight { get; set; } = 64;
	[Property, Group("Camera")] public float CameraHeightCrouching { get; set; } = 64;
	[Property, Group("Camera"), HideIf("UsePreferenceFOV", true)] public float FOV { get; set; } = 90;
	[Property, Group("Camera")] public bool UsePreferenceFOV { get; set; } = true;


	[Property, Feature("Debug")] public Vector3 WishDirection => Input.AnalogMove.ClampLength(1);
	[Property, Feature("Debug")] public Angles WishLook => Input.AnalogLook;
	[Property, Feature("Debug")] public Vector3 Gravity => Scene.PhysicsWorld.Gravity;
	[Property, Feature("Debug")] public CameraComponent Camera { get; private set; }

	private float currentCamHeight = 64;

	protected override void OnStart() {
		Camera = GameObject.GetComponentInChildren<CameraComponent>();
	}

	protected override void OnFixedUpdate() {
		var velocity = WishDirection * WalkSpeed;

		var movementModifier = updateMovementModifier();

		velocity *= movementModifier;
		velocity *= LocalRotation;

		if (Controller.IsOnGround) {
			Controller.Velocity = Controller.Velocity.WithZ(0);

			Controller.Accelerate(velocity);
			Controller.ApplyFriction(Friction);
		} else {
			Controller.Velocity += Gravity * Time.Delta * 0.5f;
			Controller.Accelerate(WishDirection);
			Controller.ApplyFriction(AirFriction);
		}

		if (Input.Pressed("Jump") && Controller.IsOnGround) Controller.Punch(Vector3.Up * JumpPower * movementModifier);

		Controller.Move();
	}

	private float updateMovementModifier() {
		float modifier = 1;

		if (Input.Down("Duck")) return modifier / 2;
		if (Input.Down("Run")) modifier *= 1.5f;

		return modifier;
	}

	protected override void OnUpdate() {
		updateInteraction();

		if (Camera is null) return;

		var camAngles = Camera.LocalRotation.Angles();
		camAngles.pitch += WishLook.pitch;
		camAngles.pitch = camAngles.pitch.Clamp(-80, 80);
		Camera.LocalRotation = camAngles;

		// set yaw (rotate the character)
		var plrAngles = LocalRotation.Angles();
		plrAngles.yaw += WishLook.yaw;
		LocalRotation = plrAngles.ToRotation();

		var fov = UsePreferenceFOV ? Preferences.FieldOfView : FOV;
		Camera.FieldOfView = fov;

		// update crouching
		var height = Input.Down("Duck") ? CameraHeightCrouching : CameraHeight;
		currentCamHeight = currentCamHeight.LerpTo(height, 0.1f);
		Camera.LocalPosition = Camera.LocalPosition.WithZ(currentCamHeight);
	}

	protected override void DrawGizmos() {
		Gizmo.Draw.SolidBox(Controller.BoundingBox);
	}
}
