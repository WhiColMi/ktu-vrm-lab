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

        Node _vfxNode;
        Light3D _lightNode;
        Timer _timer;

        public override void _Ready()
        {
            base._Ready();
            _vfxNode = GetNode(VfxNodePath);
            _lightNode = GetNode(LightNodePath) as Light3D;
            _timer = new Timer();
            AddChild(_timer);
            _timer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));

            RetrieveMeta();
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
            if (Duration == 0)
            {
                return;
            }

            if (_vfxNode is GpuParticles3D gpuParticles3D)
            {
                gpuParticles3D.Emitting = true;
                _Activate();
            }
            else if (_vfxNode is CpuParticles3D cpuParticles3D)
            {
                cpuParticles3D.Emitting = true;
                _Activate();
            }
            else
            {
                GD.PrintErr("VFX node is not a particle system.");
            }
        }

        public void Deactivate()
        {
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
        }

        void _Activate()
        {
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