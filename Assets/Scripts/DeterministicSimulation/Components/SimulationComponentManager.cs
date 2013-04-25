using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class SimulationComponentManager
	{
		protected Simulation simulation;

		public void InitDependencies(Simulation simulation)
		{
			this.simulation = simulation;
			
			OnInitDependencies();
		}

		public void Init()
		{
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

		public void ComponentAdded(SimulationComponent component)
		{
			OnComponentAdded(component);
		}

		public void ComponentRemoved(SimulationComponent component)
		{
			OnComponentRemoved(component);
		}

		protected virtual void OnInitDependencies()
		{
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

		protected virtual void OnComponentAdded (SimulationComponent component)
		{
		}
		
		protected virtual void OnComponentRemoved (SimulationComponent component)
		{
		}
	}
}

