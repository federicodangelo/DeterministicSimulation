using System;

namespace DeterministicSimulation
{
	public class SimulationEntity
	{
		protected Simulation simulation;
		protected int id;
		protected Parameters parameters;

		public int Id
		{
			get { return id; }
		}
		
		public void Init(Simulation simulation, Parameters parameters, int id)
		{
			this.simulation = simulation;
			this.id = id;
			this.parameters = parameters;
			
			OnInit();
		}

		public bool Destroyed
		{
			get { return simulation == null; }
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

		protected virtual void OnInit()
		{
			
		}
		
		protected virtual void OnReset()
		{
			
		}

		protected virtual void OnUpdate()
		{

		}
	}
}

