using Godot;
using Godot.Collections;

namespace Interactions.Character
{
    public partial class InteractionManager : Camera3D
    {
        [Export]
        public float InteractionDistance = 5.0f;

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
                if (result == grasp.Holding || !grasp.GraspStart(result))
                {
                    grasp.GraspEnd();
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