using Interactions.Character;

namespace Interactions
{
    public interface IUsable
    {
        public void OnStartUse(UseManager use);
        public void OnStopUse(){}
    }
}