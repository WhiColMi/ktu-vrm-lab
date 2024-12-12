using Godot;
using Common;
using System.Collections.Generic;

namespace Interactions
{
    public partial class Holdable : RigidBody3D
    {
        Holder Holder;
        Node originalParent;

        AudioStreamPlayer3D PickupSoundPlayer;
        AudioStreamPlayer3D DropSoundPlayer;

        public Delegates.BooleanDelegate onHoldStateChanged;

        public override void _Ready()
        {
            base._Ready();

            PickupSoundPlayer = GetNode<AudioStreamPlayer3D>("PickupSoundPlayer");
            DropSoundPlayer = GetNode<AudioStreamPlayer3D>("DropSoundPlayer");

            originalParent = GetParent();
        }

        public override void _PhysicsProcess(double delta) => CollisionDetection();



        // === Custom methods ===
        public void PickUp(Holder holder)
        {
            if (IsHeld())
            {
                return;
            }

            Holder = holder;
            Holder.Attach(this);

            OnPickUp();

            HoldStateChanged();
        }

        public void Drop()
        {
            if (!IsHeld())
            {
                return;
            }

            Holder _holder = Holder;
            Holder = null;
            _holder.Detach();

            OnDrop();

            HoldStateChanged();
        }

        public void OnPickUp() => PickupSoundPlayer.Play();
        public void OnDrop() { }

        void HoldStateChanged()
        {
            onHoldStateChanged?.Invoke(IsHeld());
        }



        // === Collision detection ===

        HashSet<Node> _currentCollisions = new HashSet<Node>();
        HashSet<Node> _previousCollisions = new HashSet<Node>();

        void CollisionDetection()
        {
            // Swap current and previous sets
            (_previousCollisions, _currentCollisions) = (_currentCollisions, _previousCollisions);
            _currentCollisions.Clear();

            // Check for new collisions
            foreach (Node body in GetCollidingBodies())
            {
                _currentCollisions.Add(body);

                if (!_previousCollisions.Contains(body))
                {
                    OnNewCollision();
                    break;
                }
            }
        }

        void OnNewCollision()
        {
            DropSoundPlayer.Play();
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

