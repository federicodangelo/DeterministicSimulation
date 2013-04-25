using UnityEngine;
using DeterministicSimulation;
using System.Collections.Generic;

public class SimulationTest : MonoBehaviour, ISimulationListener
{
	public int sizeX = 64;
	public int sizeY = 16;
	public int sizeZ = 64;

	public VoxelWorldView voxelView;

	private Simulation simulation;
	private Dictionary<int, VoxelEntityView> voxelEntityViews = new Dictionary<int, VoxelEntityView>();

	public void Start()
	{
		simulation = new Simulation();

		simulation.AddComponentManager<DeterministicRandom>();
		simulation.AddComponentManager<SimulationBehaviorManager>();
		simulation.AddComponentManager<VoxelPathFinder>();
		simulation.AddComponentManager<VoxelWorld>();

		InitSimulation();
	}

	private void InitSimulation()
	{
		simulation.AddParameter(VoxelWorld.PARAMETER_WORLD_SIZE, new IntVector3(sizeX, sizeY, sizeZ));
		simulation.AddParameter(Simulation.PARAMETER_STEPS_PER_SECOND, 20);
		simulation.AddParameter(Simulation.PARAMETER_SIMULATION_LISTENER, this);
		
		simulation.Init();
		
		voxelView.Init(simulation.GetComponentManager<VoxelWorld>());

		EntityTemplate template1 = new EntityTemplate();

		template1.AddComponent<VoxelComponent>()
			.AddParameter(VoxelComponent.PARAMETER_SHAPE, VoxelShape.Sphere)
			.AddParameter(VoxelComponent.PARAMETER_RADIUS, fint.quarter)
			.AddParameter(VoxelComponent.PARAMETER_COLOR, new FVector3(fint.one, fint.zero, fint.zero))
			.AddParameter(VoxelComponent.PARAMETER_POSITION, FVector3.one + FVector3.up * fint.half + FVector3.forward);

		template1.AddComponent<VoxelPathWalker>();
		template1.AddComponent<Avatar>();

		EntityTemplate template2 = new EntityTemplate();

		template2.AddComponent<VoxelComponent>()
			.AddParameter(VoxelComponent.PARAMETER_SHAPE, VoxelShape.Cylinder)
			.AddParameter(VoxelComponent.PARAMETER_RADIUS, fint.quarter)
			.AddParameter(VoxelComponent.PARAMETER_HEIGHT, fint.one)
			.AddParameter(VoxelComponent.PARAMETER_COLOR, new FVector3(fint.zero, fint.one, fint.zero))
			.AddParameter(VoxelComponent.PARAMETER_POSITION, FVector3.one + FVector3.up * fint.half);

		template2.AddComponent<AvatarMoveRandom>();

		for (int i = 0; i < 100; i++)
		{
			simulation.AddEntity(template1);

			simulation.AddEntity(template2);
		}
	}

	public void EntityAdded (SimulationEntity entity)
	{
		if (entity.GetComponent<VoxelComponent>() != null)
		{
			GameObject go = new GameObject();
			go.transform.parent = voxelView.transform;
			VoxelEntityView voxelEntityView = go.AddComponent<VoxelEntityView>();

			voxelEntityView.Init(entity.GetComponent<VoxelComponent>());

			voxelEntityViews.Add(entity.Id, voxelEntityView);
		}
	}
	
	public void EntityRemoved (SimulationEntity entity)
	{
		if (entity.GetComponent<VoxelComponent>() != null)
		{
			VoxelEntityView voxelEntityView = voxelEntityViews[entity.Id];

			voxelEntityViews.Remove(entity.Id);

			GameObject.Destroy(voxelEntityView.gameObject);
		}
	}

	public void Update()
	{
		simulation.Update(fint.CreateFromFloat(Time.deltaTime));

		if (Input.GetKeyDown(KeyCode.R))
		{
			simulation.Reset();

			InitSimulation();
		}
	}
}
