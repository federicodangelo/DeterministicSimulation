using System;
using DeterministicSimulation;

public class Avatar : SimulationBehavior
{
	private DeterministicRandom random;
	private VoxelPathWalker pathWalker;
	private VoxelWorld world;

	protected override void OnInit (Parameters parameters)
	{
		random = Entity.Simulation.GetComponentManager<DeterministicRandom>();
		pathWalker = Entity.GetComponent<VoxelPathWalker>();
		world = Entity.Simulation.GetComponentManager<VoxelWorld>();
	}

	protected override void OnUpdate ()
	{
		if (!pathWalker.Walking)
		{
			pathWalker.WalkTo(
				new FVector3(
					random.Range(fint.zero, fint.CreateFromInt(world.Size.x)),
					fint.one,
		             random.Range(fint.zero, fint.CreateFromInt(world.Size.z))
				)
			);
		}
	}

}


