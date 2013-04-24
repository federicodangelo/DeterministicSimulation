using UnityEngine;
using DeterministicSimulation;

public class VoxelWorldTest : MonoBehaviour
{
	public int sizeX = 64;
	public int sizeY = 16;
	public int sizeZ = 64;

	public VoxelWorldView voxelView;

	public void Start()
	{
		VoxelWorld world = new VoxelWorld();

		world.Create(sizeX, sizeY, sizeZ, 0);

		for (int x = 0; x < sizeX; x++)
			for (int z = 0; z < sizeZ; z++)
				world.SetVoxel(x, 0, z, 1);

		voxelView.Init(world);
	}
}


