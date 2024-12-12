using Godot;
using Interactions;
using Interactions.Character;

public partial class Book : Holdable, IUsable
{
    [Export]
    public NodePath UIPath;

    AudioStreamPlayer3D UseSoundPlayer;

    public override void _Ready()
    {
        base._Ready();

        UseSoundPlayer = GetNode<AudioStreamPlayer3D>("UseSoundPlayer");
    }

    public void OnStartUse(UseManager useManager)
    {
        UseSoundPlayer.Play();
        GetNode<Control>(UIPath).Show();
    }

}