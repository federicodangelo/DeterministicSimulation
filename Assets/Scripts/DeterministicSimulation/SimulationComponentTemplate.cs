using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class SimulationComponentTemplate<TEntity> : SimulationComponent where TEntity : SimulationEntity
	{
		protected List<TEntity> entities = new List<TEntity>();

		protected override void OnEntityAdded (SimulationEntity entity)
		{
			if (entity is TEntity)
				entities.Add((TEntity) entity);
		}

		protected override void OnEntityRemoved (SimulationEntity entity)
		{
			if (entity is TEntity)
				entities.Remove((TEntity) entity);
		}
	}
}

