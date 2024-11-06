using Godot;
using System.Collections.Generic;
using Common;

namespace Interactions.Environment
{
    public partial class ItemSlot : CollisionDetector
    {
        Holdable _item;
        Generic6DofJoint3D _joint;

        readonly Dictionary<Holdable, Delegates.BooleanDelegate> candidates = new();

        public override void _Ready()
        {
            base._Ready();
            _joint = new Generic6DofJoint3D();
            AddChild(_joint);
        }

        public void AttachHoldable(Holdable itemToAttach)
        {
            if (_item != null)
            {
                GD.Print("ItemSlot already has a Holdable attached.");
                return;
            }

            _item = itemToAttach;
            _joint.NodeA = GetPath();
            _joint.NodeB = itemToAttach.GetPath();
            
            RefreshCandidates();
        }

        public void DetachHoldable()
        {
            if (_item == null)
            {
                GD.Print("No Holdable attached to ItemSlot.");
                return;
            }

            _joint.NodeB = null;
            _item = null;
        }

        protected override void OnBodyEntered(Node body)
        {
            if (body is Holdable itemToAttach)
            {
                if (itemToAttach.IsHeld())
                {
                    RegisterCandidate(itemToAttach);
                }
                else
                {
                    AttachHoldable(itemToAttach);
                }
            }
        }

        void RegisterCandidate(Holdable candidate)
        {
            if (!candidates.ContainsKey(candidate))
            {
                void OnStateChanged(bool newState)
                {
                    if (newState)
                    {
                        return;
                    }

                    AttachHoldable(candidate);
                }
                candidate.onHoldStateChanged += OnStateChanged;
                candidates.Add(candidate, OnStateChanged);
            }
        }

        void RefreshCandidates()
        {
            foreach (KeyValuePair<Holdable, Delegates.BooleanDelegate> candidate in candidates)
            {
                candidate.Key.onHoldStateChanged -= candidate.Value;
            }
            candidates.Clear();
        }
    }
}
