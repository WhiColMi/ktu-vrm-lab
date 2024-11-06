using Godot;
using System;

namespace Interactions.Character
{
    public partial class GraspManager : Holder
    {
        public bool GraspStart(Node node)
        {
            if (node is Holdable holdable)
            {
                Attach(holdable);
                return true;
            }
            return false;
        }

        public void GraspEnd()
        {
            Detach();
        }
    }
}