using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class VoxelPathFinder : SimulationComponentManager
	{
		private VoxelWorld voxelWorld;

		protected override void OnInitDependencies ()
		{
			voxelWorld = simulation.GetComponentManager<VoxelWorld>();
		}

		public List<FVector3> FindPathTo(FVector3 from, FVector3 to)
		{
			List<FVector3> path = new List<FVector3>();

			//IntVector3 fromInt = from.ToIntVector3();
			//IntVector3 toInt = to.ToIntVector3();

			//TODO: Implement path finder!
			path.Add(to);

			return path;
		}
	}
}

