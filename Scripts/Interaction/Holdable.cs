using Godot;

namespace Interactions
{
    public partial class Holdable : Interactable
    {
        Holder Holder;
        Node originalParent;

        public override void InteractStart(Interactor interactor)
        {
            PickUp(interactor.GetHolder());
        }


// === Custom methods ===
        public void PickUp(Holder holder)
        {
            if (!IsHeld())
            {
                Holder = holder;
                Holder.Attach(this);
            }
        }

        public void Drop()
        {
            if (IsHeld())
            {
                Holder _holder = Holder;

                Holder = null;
                
                _holder.Detach();
            }
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

