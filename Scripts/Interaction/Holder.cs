using Godot;
using System;

namespace Interactions
{
    public partial class Holder : Generic6DofJoint3D
    {
        [Export] public NodePath anchorNodePath;

        public Node3D Anchor { get; private set; }
        public Holdable Holding { get; private set; }

        public override void _Ready()
        {
            Anchor = GetNode<Node3D>(anchorNodePath);
            NodeA = Anchor.GetPath();
        }

        public void Attach(Holdable holdable)
        {
            if (IsHolding())
            {
                Detach();
            }

            Holding = holdable;
            NodeB = holdable.GetPath();
        }

        public void Detach()
        {
            if (Holding == null)
            {
                return;
            }

            Holdable _holding = Holding;

            Holding = null;
            NodeB = null;
            
            _holding.Drop();
        }

// === Getters ===
        public bool IsHolding()
        {
            return Holding != null;
        }
    }
}