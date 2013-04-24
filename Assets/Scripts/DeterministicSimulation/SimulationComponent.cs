using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class SimulationComponent
	{
		protected Simulation simulation;

		public void Init(Simulation simulation)
		{
			this.simulation = simulation;

			OnInit();
		}

		public void Reset()
		{
			OnReset();
			
			this.simulation = null;
		}

		public void Update()
		{
			OnUpdate();
		}

		public void EntityAdded(SimulationEntity entity)
		{
			OnEntityAdded(entity);
		}

		public void EntityRemoved(SimulationEntity entity)
		{
			OnEntityRemoved(entity);
		}

		protected virtual void OnInit()
		{

		}

		protected virtual void OnReset()
		{

		}

		protected virtual void OnUpdate()
		{

		}

		protected virtual void OnEntityAdded(SimulationEntity entity)
		{

		}

		protected virtual void OnEntityRemoved(SimulationEntity entity)
		{
			
		}
	}
}

