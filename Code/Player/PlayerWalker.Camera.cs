public partial class PlayerWalker {
	[Property, Group("Camera")] public float CameraHeight { get; set; } = 64;
	[Property, Group("Camera")] public float CameraHeightCrouching { get; set; } = 64;
	[Property, Group("Camera"), HideIf("UsePreferenceFOV", true)] public float FOV { get; set; } = 90;
	[Property, Group("Camera")] public bool UsePreferenceFOV { get; set; } = true;

	[Property, Group("Camera Wobble")] public float WobbleTime { get; set; } = 6f;

	private float currentCamHeight = 64;

	private void updateCamera() {
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

		var sin = getWobbleFactor();

		currentCamHeight = currentCamHeight.LerpTo(height + sin, 0.1f);
		Camera.LocalPosition = Camera.LocalPosition.WithZ(currentCamHeight);
	}

	private float getWobbleFactor() {
		if (WishDirection.Length == 0) return 1;

		var time = Time.Now * WobbleTime * MovementMultiplier;

		return MathF.Sin(time);
	}
}
