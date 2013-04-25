using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class VoxelPathWalker : SimulationBehavior
	{
		private bool walking;

		private VoxelComponent voxelComponent;
		private VoxelPathFinder voxelPathFinder;

		private List<FVector3> path = new List<FVector3>();
		private int pathIndex;
		private fint speed = fint.CreateFromInt(10);

		protected override void OnInitDependencies ()
		{
			voxelPathFinder = Entity.Simulation.GetComponentManager<VoxelPathFinder>();
			voxelComponent = Entity.GetComponent<VoxelComponent>();
		}

		public bool Walking
		{
			get { return walking; }
		}

		public void WalkTo(FVector3 target)
		{
			List<FVector3> path = voxelPathFinder.FindPathTo(voxelComponent.position, target);
			if (path != null && path.Count > 0)
			{
				this.path = path;
				this.pathIndex = 0;
				this.walking = true;
			}
		}

		protected override void OnUpdate ()
		{
			if (walking)
			{
				FVector3 nextPoint = path[pathIndex];

				nextPoint.y = voxelComponent.position.y;
				
				voxelComponent.position = FVector3.MoveTowards(voxelComponent.position, nextPoint, SimulationTime.deltaTime * speed);
				
				if (voxelComponent.position == nextPoint)
				{
					pathIndex++;
					if (pathIndex == path.Count)
					{
						path.Clear();
						pathIndex = 0;
						walking = false;
					}
				}
			}
		}
	}
}

