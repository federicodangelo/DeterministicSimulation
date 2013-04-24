using System;

namespace DeterministicSimulation
{
	public interface ISimulationListener
	{
		void EntityAdded(SimulationEntity entity);

		void EntityRemoved(SimulationEntity entity);
	}
}

