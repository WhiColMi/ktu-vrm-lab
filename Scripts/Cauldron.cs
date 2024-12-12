using Godot;
using Interactions.Environment;

public partial class Cauldron : FlammableCondition
{
    [Export]
    public NodePath CauldronContentPath;

    AudioStreamPlayer3D BrewingSoundPlayer;

    MeshInstance3D CauldronFillMesh;


    public override void _Ready()
    {
        base._Ready();
        Connect(nameof(StateChanged), new Callable(this, nameof(OnStateChanged)));

        BrewingSoundPlayer = GetNode<AudioStreamPlayer3D>("BrewingSoundPlayer");
        CauldronFillMesh = GetNode<MeshInstance3D>("CauldronFillMesh");

        Deactivate();
    }

    void OnStateChanged(bool value)
    {
        if (value)
        {
            Activate();
        }
    }

    void Activate()
    {
        BrewingSoundPlayer.Play();
        CauldronFillMesh.Show();
        if (GetNode(CauldronContentPath) is Node3D CauldronContent)
        {
            CauldronContent.Visible = true;
        }
    }

    void Deactivate()
    {
        BrewingSoundPlayer.Stop();
        CauldronFillMesh.Hide();
        if (GetNode(CauldronContentPath) is Node3D CauldronContent)
        {
            CauldronContent.Visible = false;
        }
    }
}
