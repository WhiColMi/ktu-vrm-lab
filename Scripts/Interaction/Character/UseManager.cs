using Godot;
using Godot.Collections;

namespace Interactions.Character
{
    public partial class UseManager : Node
    {
        IUsable inUseWith;

        public bool UseStart(Node node)
        {
            if(node is IUsable usable)
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