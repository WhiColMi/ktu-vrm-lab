using Godot;
using Godot.Collections;

namespace Interactions.Character
{
    public partial class InteractionManager : Camera3D
    {
        [Export]
        public float InteractionDistance = 5.0f;

        [Export(PropertyHint.Layers3DPhysics)]
        public uint interactionMask = (1 << 0) | (1 << 2);

        GraspManager grasp;
        UseManager use;

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

            grasp = GetNode<GraspManager>("./Grasp");
            use = GetNode<UseManager>("./Use");
        }

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed("use"))
            {
                Node result = RayCast();
                if (!use.UseStart(result))
                {
                    use.UseStart(grasp.Holding);
                }
            }
            if (Input.IsActionJustReleased("use"))
            {
                use.UseEnd();
            }

            if (Input.IsActionJustPressed("grasp"))
            {
                Node result = RayCast();

                // If the result is same as the current holding object, release it
                if (result == grasp.Holding)
                {
                    grasp.Detach();
                }
                // Otherwise, check if the result is a holdable object and attach it
                else if (result is Holdable holdable)
                {
                    grasp.Attach(holdable);
                }
                // Otherwise, release the current holding object
                else
                {
                    grasp.Detach();
                }
            }
        }



// === Custom methods ===

        Node RayCast()
        {
            var from = GlobalTransform.Origin;
            var to = from - GlobalTransform.Basis.Column2 * InteractionDistance;

            Dictionary result = GetViewport().GetWorld3D().DirectSpaceState.IntersectRay(
                new PhysicsRayQueryParameters3D()
                {
                    CollideWithAreas = true,
                    CollideWithBodies = true,
                    CollisionMask = interactionMask,
                    Exclude = excludedInInteraction,
                    From = from,
                    HitBackFaces = true,
                    HitFromInside = true,
                    To = to,
                }
            );

            if (result.Count > 0)
            {
                if (result["collider"].Obj is Node collider)
                {
                    return collider;
                }
            }
            else
            {
                GD.Print("No interaction target found");
            }

            return null;
        }
    }
}