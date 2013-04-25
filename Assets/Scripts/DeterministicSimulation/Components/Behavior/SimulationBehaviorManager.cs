using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class SimulationBehaviorManager : SimulationComponentManagerTemplate<SimulationBehavior>
	{
		protected override void OnUpdateComponents(List<SimulationBehavior> components)
		{
			for (int i = 0; i < components.Count; i++)
				components[i].Update();
		}
	}
}

