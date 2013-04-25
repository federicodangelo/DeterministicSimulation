using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public sealed class SimulationEntity
	{
		private Simulation simulation;
		private int id;
		internal List<SimulationComponent> components = new List<SimulationComponent>();

		public int Id
		{
			get { return id; }
		}

		public Simulation Simulation
		{
			get { return simulation; }
		}
		
		internal void Init(Simulation simulation, int id, EntityTemplate template)
		{
			this.simulation = simulation;
			this.id = id;

			for (int i = 0; i < template.componentTemplates.Count; i++)
				components.Add(template.componentTemplates[i].CreateComponent());

			for (int i = 0; i < components.Count; i++)
				components[i].InitDependencies(this);

			for (int i = 0; i < components.Count; i++)
				components[i].Init(template.componentTemplates[i].parameters);
		}

		internal void Destroy()
		{
			if (components.Count > 0)
			{
				for (int i = 0; i < components.Count; i++)
					components[i].Destroy();
				components.Clear();
			}

			simulation = null;
		}

		public bool Destroyed
		{
			get { return simulation == null; }
		}
		
		public T GetComponent<T>() where T : SimulationComponent
		{
			for (int i = 0; i < components.Count; i++)
				if (components[i] is T)
					return (T) components[i];

			return null;
		}
	}
}

