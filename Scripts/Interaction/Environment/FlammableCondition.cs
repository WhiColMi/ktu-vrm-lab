using System.Diagnostics;
using Godot;

namespace Interactions.Environment
{
    public partial class FlammableCondition : Node3D
    {

        [Export]
        public NodePath[] flammables;

        [Signal]
        public delegate void StateChangedEventHandler(bool state);

        Flammable[] flammableNodes;

        bool _state;
        bool state {
            get => _state;
            set {
                if (_state != value)
                {
                    _state = value;
                    EmitSignal(nameof(StateChanged), _state);
                }
            }
        }


        public override void _EnterTree()
        {
            base._EnterTree();

            flammableNodes = new Flammable[flammables.Length];
            for (int i = 0; i < flammables.Length; i++)
            {
                flammableNodes[i] = GetNode(flammables[i]).GetNode<Flammable>("Flammable");
                if (flammableNodes[i] != null)
                {
                    flammableNodes[i].Connect(nameof(Flammable.FireActivated), new Callable(this, nameof(UpdateState)));
                    flammableNodes[i].Connect(nameof(Flammable.FireDeactivated), new Callable(this, nameof(UpdateState)));
                }
            }
        }

        void UpdateState()
        {
            if (state)
            {
                foreach (Flammable flammable in flammableNodes)
                {
                    if (!flammable.IsActivated())
                    {
                        state = false;
                        return;
                    }
                }
            }
            else
            {
                foreach (Flammable flammable in flammableNodes)
                {
                    if (!flammable.IsActivated())
                    {
                        return;
                    }
                }
                state = true;
            }
        }

    }
}
