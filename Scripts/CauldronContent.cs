using Godot;
using Interactions.Environment;

public partial class CauldronContent : CollisionDetector
{
    public override void _Ready()
    {
        base._Ready();
    }

    protected override void OnBodyEntered(Node body)
    {
        GD.Print($"CauldronContent.OnBodyEntered: {body.Name}");
        if (body is Bottle bottle)
        {
            bottle.Fill();
        }
    }
}
