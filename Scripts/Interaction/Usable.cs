using System;
using Interactions.Character;

namespace Interactions
{
    public abstract partial class Usable : Holdable
    {
        public abstract void OnStartUse(UseManager use);
        public virtual void OnStopUse(){}
    }
}