using Godot;
using Godot.Collections;

namespace Interactions.Character
{
    public partial class UseManager : Node
    {
        Usable inUseWith;

        public bool UseStart(Node node)
        {
            if(node is Usable usable)
            {
                usable.OnStartUse(this);
                inUseWith = usable;
                return true;
            }
            return false;
        }
    
        public void UseEnd()
        {
            if(inUseWith != null)
            {
                inUseWith.OnStopUse();
                inUseWith = null;
            }
        }
    }
}