using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class EntityTemplate
	{
		public abstract class ComponentTemplate
		{
			public Parameters parameters = new Parameters();

			public abstract SimulationComponent CreateComponent();
		}

		public class ComponentTemplate<T>  : ComponentTemplate where T : SimulationComponent, new()
		{
			public override SimulationComponent CreateComponent()
			{
				return new T();
			}
		}

		public List<ComponentTemplate> componentTemplates = new List<ComponentTemplate>();

		public Parameters AddComponent<T>() where T : SimulationComponent, new()
		{
			ComponentTemplate<T> template = new ComponentTemplate<T>();

			componentTemplates.Add(template);

			return template.parameters;
		}
	}
}

