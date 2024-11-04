using Godot;

public partial class CharacterController : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float JumpForce = 10.0f;

	[Export]
	public float MouseSensitivity = 0.1f;
	[Export]
	public Node3D CameraOriginNode;


	Vector3 _velocity = Vector3.Zero;
	Vector2 _mouseDelta = Vector2.Zero;
	float yaw = 0;


	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			_mouseDelta = mouseMotion.Relative;
		}
	}

	public override void _Process(double delta)
	{
		RotateY(Mathf.DegToRad(-_mouseDelta.X * MouseSensitivity));
		yaw = Mathf.Clamp(yaw - _mouseDelta.Y * MouseSensitivity, -89, 89);
		CameraOriginNode.Transform = new Transform3D(new Basis(Vector3.Right, Mathf.DegToRad(yaw)), CameraOriginNode.Transform.Origin);
		_mouseDelta = Vector2.Zero;
	}

    public override void _PhysicsProcess(double delta)
	{
		Vector3 direction = Vector3.Zero;

		if (Input.IsActionPressed("move_forward"))
			direction -= Transform.Basis.Column2;
		if (Input.IsActionPressed("move_backward"))
			direction += Transform.Basis.Column2;
		if (Input.IsActionPressed("move_left"))
			direction -= Transform.Basis.Column0;
		if (Input.IsActionPressed("move_right"))
			direction += Transform.Basis.Column0;

		direction = direction.Normalized();
		_velocity.X = direction.X * Speed;
		_velocity.Z = direction.Z * Speed;

		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			_velocity.Y = JumpForce;
		}
		else if (!IsOnFloor())
		{
			_velocity.Y += (float)(-9.8 * delta); // Apply gravity
		}

		Velocity = _velocity;
		MoveAndSlide();
	}
}