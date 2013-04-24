using System;
using DeterministicSimulation;

public class Avatar : VoxelEntity
{
	private FVector3 randomTarget;
	private DeterministicRandom random;

	protected override void OnInit ()
	{
		base.OnInit ();

		random = simulation.GetComponent<DeterministicRandom>();

		SelectTarget();
	}

	protected override void OnUpdate ()
	{
		randomTarget.y = position.y;

		position = FVector3.MoveTowards(position, randomTarget, SimulationTime.deltaTime * fint.CreateFromInt(10));

		if (position == randomTarget)
			SelectTarget();
	}

	private void SelectTarget()
	{
		randomTarget = 
			new FVector3(voxelWorld.Size.ToFVector3().x * random.Value,
			             position.y,
			             voxelWorld.Size.ToFVector3().z * random.Value);
	}
}


