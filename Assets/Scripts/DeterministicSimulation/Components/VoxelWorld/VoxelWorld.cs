#define VALIDATE_PARAMETERS

using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class VoxelWorld : SimulationComponentManagerTemplate<VoxelComponent>
	{
		public const string PARAMETER_WORLD_SIZE = "worldSize";

		private IntVector3 size;
		private byte[] voxels;
		private DeterministicRandom random;

		public IntVector3 Size
		{
			get { return size; }
		}

		protected override void OnInitDependencies ()
		{
			random = simulation.GetComponentManager<DeterministicRandom>();
		}

		protected override void OnInit ()
		{
			IntVector3 size = simulation.GetParameter<IntVector3>(PARAMETER_WORLD_SIZE, new IntVector3(0, 0, 0));

			if (size.x < 0 || size.y < 0 || size.z < 0)
				throw new System.ArgumentOutOfRangeException(PARAMETER_WORLD_SIZE, size, "Invalid size");

			voxels = new byte[size.x * size.y * size.z];

			this.size = size;

			//Create "floor"
			for (int x = 0; x < size.x; x++)
				for (int z = 0; z < size.z; z++)
					SetVoxel(x, 0, z, 1);

			//Create "obstacles" on second floor
			for (int i = 0; i < (size.x * size.y) / 2; i++)
			{
				SetVoxel(
					random.Range(0, size.x),
					1,
					random.Range(0, size.z),
					2);
			}
		}

		public bool IsValidPosition(int x, int y, int z)
		{
			return 
				x >= 0 && x < size.x &&
				y >= 0 && y < size.y &&
				z >= 0 && z < size.z;
		}

		public byte GetVoxel(int x, int y, int z)
		{
			#if VALIDATE_PARAMETERS
			if (x < 0 || x >= size.x)
				throw new System.ArgumentOutOfRangeException("x", x, "Invalid position");
			if (y < 0 || y >= size.y)
				throw new System.ArgumentOutOfRangeException("y", y, "Invalid position");
			if (z < 0 || z >= size.z)
				throw new System.ArgumentOutOfRangeException("z", z, "Invalid position");
			#endif

			return voxels[x + y * (size.x) + z * (size.x * size.y)];
		}

		public void SetVoxel(int x, int y, int z, byte voxel)
		{
			#if VALIDATE_PARAMETERS
			if (x < 0 || x >= size.x)
				throw new System.ArgumentOutOfRangeException("x", x, "Invalid position");
			if (y < 0 || y >= size.y)
				throw new System.ArgumentOutOfRangeException("y", y, "Invalid position");
			if (z < 0 || z >= size.z)
				throw new System.ArgumentOutOfRangeException("z", z, "Invalid position");
			#endif
			
			voxels[x + y * (size.x) + z * (size.x * size.y)] = voxel;
		}

		protected override void OnUpdateComponents(List<VoxelComponent> components)
		{
			for (int i = 0; i < components.Count; i++)
			{
				VoxelComponent voxel = components[i];

				MoveAboveFloor(voxel);

				if (voxel.useGravity)
					voxel.position.y -= fint.CreateFromInt(10) * SimulationTime.deltaTime;

				MoveAboveFloor(voxel);
			}
		}

		private void MoveAboveFloor(VoxelComponent voxel)
		{
			FVector3 position = voxel.position;
			fint radius = voxel.radius;
			
			FVector3 deltaBottom;
			
			if (voxel.shape == VoxelShape.Sphere)
				deltaBottom = FVector3.down * radius;
			else 
				deltaBottom = FVector3.down * voxel.height * fint.half;
			
			FVector3 bottom = position + deltaBottom;
			
			int x = bottom.x.ToInt();
			int y = bottom.y.ToInt();
			int z = bottom.z.ToInt();
			
			if (IsValidPosition(x, y, z))
			{
				if (GetVoxel(x, y, z) != 0 && IsValidPosition(x, y + 1, z))
				{
					position.y = fint.CreateFromInt(y + 1);
					
					voxel.position.y = position.y - deltaBottom.y;
				}
			}
		}
	}
}