using Godot;
using System;

namespace Interactions.Character
{
    public partial class GraspManager : Holder
    {
        [Export(PropertyHint.Layers3DPhysics)]
        public uint ignoreMask = 1 << 1;

        float mass;
        float gravityScale;
        uint layer;
        uint mask;

        public override void Attach(Holdable holdable)
        {
            if (IsHolding())
            {
                Detach();
            }

            mass = holdable.Mass;
            gravityScale = holdable.GravityScale;
            layer = holdable.GetCollisionLayer();
            mask = holdable.GetCollisionMask();
            holdable.Mass = 0.01f;
            holdable.GravityScale = 0.01f;
            holdable.SetCollisionLayer(1<<2);
            holdable.SetCollisionMask(mask & ~ignoreMask);

            Holding = holdable;
            UpdateJointAttachment();
        }

        public override void Detach()
        {
            if (!IsHolding())
            {
                return;
            }

            Holdable _holding = Holding;

            Holding = null;
            UpdateJointAttachment();

            _holding.Drop();

            _holding.Mass = mass;
            _holding.GravityScale = gravityScale;
            _holding.SetCollisionLayer(layer);
            _holding.SetCollisionMask(mask);
            mass = 0.0f;
            gravityScale = 0.0f;
            layer = 0;
            mask = 0;
        }


    }
}