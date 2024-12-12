using Godot;

namespace Interactions.Environment
{
    public partial class Flammable : CollisionDetector
    {
        [Export]
        public NodePath VfxNodePath;
        [Export]
        public NodePath LightNodePath;

        [Export]
        public float Duration = 2.0f; // Duration in seconds

        [Signal]
        public delegate void FireActivatedEventHandler();

        [Signal]
        public delegate void FireDeactivatedEventHandler();

        Node _vfxNode;
        Light3D _lightNode;
        Timer _timer;

        AudioStreamPlayer3D BurnSoundPlayer;
        AudioStreamPlayer3D LitSoundPlayer;

        public override void _Ready()
        {
            base._Ready();

            BurnSoundPlayer = GetNode<AudioStreamPlayer3D>("BurnSoundPlayer");

            _vfxNode = GetNode(VfxNodePath);
            _lightNode = GetNode(LightNodePath) as Light3D;
            _timer = new Timer();
            AddChild(_timer);
            _timer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));

            RetrieveMeta();

            LitSoundPlayer = GetNode<AudioStreamPlayer3D>("LitSoundPlayer");
        }

        void RetrieveMeta()
        {
            Node parent = GetParent();
            if (parent.HasMeta("FireDuration"))
            {
                Duration = (float)parent.GetMeta("FireDuration");
            }
            if (parent.HasMeta("Fire"))
            {
                if ((bool)parent.GetMeta("Fire"))
                {
                    Activate();
                }
                else
                {
                    Deactivate();
                }
            }
        }

        protected override void OnBodyEntered(Node body)
        {
            foreach (Node child in body.GetChildren())
            {
                if (child is Flammable flammable)
                {
                    if (flammable.IsActivated())
                    {
                        Activate();
                        break;
                    }
                }
            }
        }


        public bool IsActivated()
        {
            return _vfxNode is GpuParticles3D gpuParticles3D && gpuParticles3D.Emitting ||
                   _vfxNode is CpuParticles3D cpuParticles3D && cpuParticles3D.Emitting;
        }

        public void Activate()
        {
            if (Duration == 0 && IsActivated())
            {
                return;
            }

            if (_vfxNode is GpuParticles3D gpuParticles3D)
            {
                _Activate();
                gpuParticles3D.Emitting = true;
            }
            else if (_vfxNode is CpuParticles3D cpuParticles3D)
            {
                _Activate();
                cpuParticles3D.Emitting = true;
            }
            else
            {
                GD.PrintErr("VFX node is not a particle system.");
            }

            EmitSignal(nameof(FireActivated));
        }

        public void Deactivate()
        {
            BurnSoundPlayer.Stop();

            if (_vfxNode is GpuParticles3D gpuParticles3D)
            {
                gpuParticles3D.Emitting = false;
            }
            else if (_vfxNode is CpuParticles3D cpuParticles3D)
            {
                cpuParticles3D.Emitting = false;
            }
            _lightNode?.Hide();
            _timer.Stop();

            EmitSignal(nameof(FireDeactivated));
        }

        void _Activate()
        {
            BurnSoundPlayer.Play();
            if (!IsActivated())
            {
                LitSoundPlayer?.Play();
            }

            _lightNode?.Show();
            if (Duration < 0)
            {
                return;
            }
            _timer.Start(Duration);
        }

        void OnTimerTimeout()
        {
            Deactivate();
        }
    }
}