using Godot;
using System;

namespace Interactions
{
    public partial class Holder : Generic6DofJoint3D
    {
        Node3D hand;
        Holdable holding;

        public override void _Ready()
        {
            hand = GetParent().GetNode<Node3D>("./Hand");
            NodeA = hand.GetPath();
        }

        public void Attach(Holdable holdable)
        {
            if (IsHolding())
            {
                Detach();
            }

            holding = holdable;
            NodeB = holdable.GetPath();
        }

        public void Detach()
        {
            if (holding == null)
            {
                return;
            }

            Holdable _holding = holding;

            holding = null;
            NodeB = null;
            
            _holding.Drop();
        }


// === Getters ===
        public Node3D GetHand()
        {
            return hand;
        }

        public bool IsHolding()
        {
            return holding != null;
        }
    }
}