using System;

namespace DeterministicSimulation
{
	public class SimulationBehavior : SimulationComponent
	{
		public void Update()
		{
			OnUpdate();
		}

		protected virtual void OnUpdate()
		{

		}
	}
}

