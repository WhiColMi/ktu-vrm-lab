using Godot;

public abstract partial class CollisionDetector : Area3D
{
    public override void _Ready()
    {
        // Connect the area_entered signal to detect when another area enters
        this.Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
    }

    protected abstract void OnBodyEntered(Node body);
}
