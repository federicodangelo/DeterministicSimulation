using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class SimulationComponentManagerTemplate<TComponent> : SimulationComponentManager where TComponent : SimulationComponent
	{
		private bool insideUpdate;

		private List<TComponent> components = new List<TComponent>();
		private List<TComponent> componentsToAdd = new List<TComponent>();
		private List<TComponent> componentsToRemove = new List<TComponent>();

		protected sealed override void OnComponentAdded (SimulationComponent component)
		{
			if (component is TComponent)
			{
				TComponent c = (TComponent) component;

				if (!insideUpdate)
				{
					components.Add(c);
				}
				else
				{
					componentsToRemove.Remove(c);
					componentsToAdd.Add(c);
				}
			}
		}

		protected sealed override void OnComponentRemoved (SimulationComponent component)
		{
			if (component is TComponent)
			{
				TComponent c = (TComponent) component;

				if (!insideUpdate)
				{
					components.Remove(c);
				}
				else
				{
					componentsToAdd.Remove(c);
					componentsToRemove.Add(c);
				}
			}
		}

		protected sealed override void OnUpdate ()
		{
			insideUpdate = true;

			OnUpdateComponents(components);

			if (componentsToAdd.Count > 0)
			{
				for (int i = 0; i < componentsToAdd.Count; i++)
					components.Add(componentsToAdd[i]);
				componentsToAdd.Clear();
			}

			if (componentsToRemove.Count > 0)
			{
				for (int i = 0; i < componentsToRemove.Count; i++)
					components.Remove(componentsToRemove[i]);
				componentsToRemove.Clear();
			}

			insideUpdate = false;
		}

		protected virtual void OnUpdateComponents(List<TComponent> components)
		{

		}
	}
}
