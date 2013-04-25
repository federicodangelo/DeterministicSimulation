using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class SimulationComponent
	{
		private SimulationEntity entity;

		public SimulationEntity Entity
		{
			get { return entity; }
		}

		public void InitDependencies(SimulationEntity entity)
		{
			this.entity = entity;

			OnInitDependencies();
		}

		internal void Init(Parameters parameters)
		{
			OnInit(parameters);
		}

		internal void Destroy()
		{
			OnDestroy();

			this.entity = null;
		}

		protected virtual void OnInitDependencies()
		{
		}

		protected virtual void OnInit(Parameters parameters)
		{
		}

		protected virtual void OnDestroy()
		{
		}
	}
}

