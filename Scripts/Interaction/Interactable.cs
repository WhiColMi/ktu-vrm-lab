using Godot;
using Interactions.Character;

namespace Interactions
{
	public abstract partial class Interactable : RigidBody3D
	{
		public abstract void InteractStart(InteractionManager interactor);
		public virtual void InteractEnd(InteractionManager interactor) { }
	}
}