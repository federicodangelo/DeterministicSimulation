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

		VoxelWorld world = simulation.AddComponent<VoxelWorld>();
		simulation.AddComponent<DeterministicRandom>();

		simulation.AddParameter(VoxelWorld.PARAMETER_WORLD_SIZE, new IntVector3(sizeX, sizeY, sizeZ));
		simulation.AddParameter(Simulation.PARAMETER_STEPS_PER_SECOND, 20);
		simulation.AddParameter(Simulation.PARAMETER_SIMULATION_LISTENER, this);

		simulation.Init();

		voxelView.Init(world);

		for (int i = 0; i < 20; i++)
		{
			simulation.AddEntity<Avatar>(new Parameters()
			                                      .AddParameter(VoxelEntity.PARAMETER_SHAPE, VoxelEntityShape.Sphere)
			                                  	  .AddParameter(VoxelEntity.PARAMETER_RADIUS, fint.quarter)
			                                      .AddParameter(VoxelEntity.PARAMETER_COLOR, new FVector3(fint.one, fint.zero, fint.zero))
				                                  .AddParameter(VoxelEntity.PARAMETER_POSITION, FVector3.one + FVector3.up * fint.half + FVector3.right));

			simulation.AddEntity<Avatar>(new Parameters()
			                                      .AddParameter(VoxelEntity.PARAMETER_SHAPE, VoxelEntityShape.Cylinder)
			                                  	  .AddParameter(VoxelEntity.PARAMETER_RADIUS, fint.quarter)
				                                  .AddParameter(VoxelEntity.PARAMETER_HEIGHT, fint.one)
			                                  	  .AddParameter(VoxelEntity.PARAMETER_COLOR, new FVector3(fint.zero, fint.one, fint.zero))
			                                 	  .AddParameter(VoxelEntity.PARAMETER_POSITION, FVector3.one + FVector3.up * fint.half));
		}
	}

	public void EntityAdded (SimulationEntity entity)
	{
		if (entity is VoxelEntity)
		{
			GameObject go = new GameObject();
			go.transform.parent = voxelView.transform;
			VoxelEntityView voxelEntityView = go.AddComponent<VoxelEntityView>();

			voxelEntityView.Init((VoxelEntity) entity);

			voxelEntityViews.Add(entity.Id, voxelEntityView);
		}
	}
	
	public void EntityRemoved (SimulationEntity entity)
	{
		if (entity is VoxelEntity)
		{
			VoxelEntityView voxelEntityView = voxelEntityViews[entity.Id];

			voxelEntityViews.Remove(entity.Id);

			GameObject.Destroy(voxelEntityView.gameObject);
		}
	}

	public void Update()
	{
		simulation.Update(fint.CreateFromFloat(Time.deltaTime));
	}
}
