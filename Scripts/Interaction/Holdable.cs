using Godot;
using Common;

namespace Interactions
{
    public partial class Holdable : RigidBody3D
    {
        Holder Holder;
        Node originalParent;

        
        public Delegates.BooleanDelegate onHoldStateChanged;


// === Custom methods ===
        public void PickUp(Holder holder)
        {
            if (!IsHeld())
            {
                Holder = holder;
                Holder.Attach(this);
                onHoldStateChanged?.Invoke(IsHeld());
                HoldStateChanged();
            }
        }

        public void Drop()
        {
            if (IsHeld())
            {
                Holder _holder = Holder;
                Holder = null;
                _holder.Detach();
                HoldStateChanged();
            }
        }

        void HoldStateChanged()
        {
            onHoldStateChanged?.Invoke(IsHeld());
        }


        // === Getters ===
        public bool IsHeld()
        {
            return Holder != null;
        }

        public Node GetHolder()
        {
            return Holder;
        }
    }
}

