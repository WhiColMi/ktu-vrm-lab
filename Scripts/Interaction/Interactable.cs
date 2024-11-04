using Godot;

namespace Interactions
{
	public abstract partial class Interactable : RigidBody3D
	{
		public abstract void InteractStart(Interactor interactor);
		public virtual void InteractEnd(Interactor interactor) { }
	}
}