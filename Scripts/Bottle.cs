using Godot;
using Interactions;
using Interactions.Character;

public partial class Bottle : Holdable, IUsable
{

    [Export]
    public NodePath LightPath;

    AudioStreamPlayer3D FillSoundPlayer;
    AudioStreamPlayer3D EmptySoundPlayer;

    MeshInstance3D BottleFillMesh;
    

    public override void _Ready()
    {
        base._Ready();

        BottleFillMesh = GetNode<MeshInstance3D>("BottleFillMesh");

        Empty();

        FillSoundPlayer = GetNode<AudioStreamPlayer3D>("FillSoundPlayer");
        EmptySoundPlayer = GetNode<AudioStreamPlayer3D>("EmptySoundPlayer");
    }
    
    public void OnStartUse(UseManager useManager)
    {
        if (useManager.GetParent().GetParent() is CharacterController characterController)
        {
            characterController.JumpForce = 12;
            Empty();
        }
        
    }

    public void Fill()
    {
        FillSoundPlayer.Play();
        BottleFillMesh.Show();
        if (GetNode(LightPath) is Light3D light3D)
        {
            light3D.Show();
        }
    }

    public void Empty()
    {
        EmptySoundPlayer?.Play();
        BottleFillMesh.Hide();
        if (GetNode(LightPath) is Light3D light3D)
        {
            light3D.Hide();
        }
        else
        {
            GD.PrintErr("Light not found");
        }
    }
}