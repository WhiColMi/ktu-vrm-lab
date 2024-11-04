using Godot;
using Godot.Collections;

namespace Interactions
{
    public partial class Interactor : Camera3D
    {
        [Export]
        public float InteractionDistance = 5.0f;

        Interactable inInteractionWith;
        Holder Holder;
        readonly Array<Rid> excludedInInteraction = new();

// === Godot methods ===

        public override void _Ready()
        {
            if (GetParent() is CollisionObject3D playerCollisionObject)
            {
                GD.Print("Player collision object found: " + playerCollisionObject.Name);
                excludedInInteraction.Add(playerCollisionObject.GetRid());
            }
            else
            {
                GD.Print("Player collision object not found");
            }

            Holder = GetNode<Holder>("./Holder");
        }            

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed("interact"))
            {
                InteractStart();
            }
            if (Input.IsActionJustReleased("interact"))
            {
                InteractEnd();
            }

            if (Input.IsActionJustPressed("drop"))
            {
                Holder.Detach();
            }
        }



// === Custom methods ===

        void InteractStart()
        {
            var from = GlobalTransform.Origin;
            var to = from - GlobalTransform.Basis.Column2 * InteractionDistance;

            Dictionary result = GetViewport().GetWorld3D().DirectSpaceState.IntersectRay(
                new PhysicsRayQueryParameters3D()
                {
                    CollideWithAreas = true,
                    CollideWithBodies = true,
                    CollisionMask = 1,
                    Exclude = excludedInInteraction,
                    From = from,
                    HitBackFaces = true,
                    HitFromInside = true,
                    To = to,
                }
            );

            if (result.Count > 0)
            {
                if (result["collider"].Obj is Node collider && collider is Interactable interactable)
                {
                    interactable.InteractStart(this);
                    inInteractionWith = interactable;
                }
            }
            else
            {
                GD.Print("No interaction target found");
            }
        }
    
        void InteractEnd()
        {
            if (inInteractionWith != null)
            {
                inInteractionWith.InteractEnd(this);
                inInteractionWith = null;
            }
        }

        public Holder GetHolder()
        {
            return Holder;
        }
    }
}