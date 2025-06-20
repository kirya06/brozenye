public class SpinnyWeeny : Component {
	[Property] public float Rotation { get; set; } = 5f;

	protected override void OnUpdate() {
		var angles = LocalRotation.Angles();
		LocalRotation = angles.WithYaw(angles.yaw + Rotation).ToRotation();
	}
}
