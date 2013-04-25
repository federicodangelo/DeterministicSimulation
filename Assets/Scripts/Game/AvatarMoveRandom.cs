using System;
using DeterministicSimulation;

public class AvatarMoveRandom : SimulationBehavior
{
	private FVector3 randomTarget;
	private DeterministicRandom random;
	private VoxelComponent voxelComponent;
	private VoxelWorld voxelWorld;
	
	protected override void OnInit (Parameters parameters)
	{
		random = Entity.Simulation.GetComponentManager<DeterministicRandom>();
		voxelComponent = Entity.GetComponent<VoxelComponent>();
		voxelWorld = voxelComponent.World;
		
		SelectTarget();
	}
	
	protected override void OnUpdate ()
	{
		randomTarget.y = voxelComponent.position.y;
		
		voxelComponent.position = FVector3.MoveTowards(voxelComponent.position, randomTarget, SimulationTime.deltaTime * fint.CreateFromInt(10));
		
		if (voxelComponent.position == randomTarget)
			SelectTarget();
	}
	
	private void SelectTarget()
	{
		randomTarget = 
			new FVector3(voxelWorld.Size.ToFVector3().x * random.Value,
			             voxelComponent.position.y,
			             voxelWorld.Size.ToFVector3().z * random.Value);
	}
}


