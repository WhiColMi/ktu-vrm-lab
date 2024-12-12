using Godot;
using Interactions;
using Interactions.Character;

public partial class Blocker : StaticBody3D, IUsable
{
    AudioStreamPlayer3D UseSoundPlayer;

    public override void _Ready()
    {
        base._Ready();

        UseSoundPlayer = GetNode<AudioStreamPlayer3D>("UseSoundPlayer");
    }

    public void OnStartUse(UseManager useManager)
    {
        UseSoundPlayer.Play();
        Visible = false;
    }
}