using Godot;
using System;

namespace Interactions
{
    public partial class Holder : Generic6DofJoint3D
    {
        [Export] public NodePath anchorNodePath;

        public Node3D Anchor { get; private set; }
        public Holdable Holding { get; protected set; }

        public override void _Ready()
        {
            Anchor = GetNode<Node3D>(anchorNodePath);
            NodeA = Anchor.GetPath();
        }

        public virtual void Attach(Holdable holdable)
        {
            if (IsHolding())
            {
                Detach();
            }

            Holding = holdable;
            UpdateJointAttachment();
        }

        public virtual void Detach()
        {
            if (!IsHolding())
            {
                return;
            }

            Holdable _holding = Holding;

            Holding = null;
            UpdateJointAttachment();

            _holding.Drop();
        }

        protected void UpdateJointAttachment()
        {
            if (Holding == null)
            {
                NodeB = null;
            }
            else
            {
                var originalTransform = Holding.Transform;
                Holding.GlobalTransform = Anchor.GlobalTransform;
                NodeB = Holding.GetPath();
                Holding.Transform = originalTransform;
            }
        }

// === Getters ===
        public bool IsHolding()
        {
            return Holding != null;
        }
    }
}