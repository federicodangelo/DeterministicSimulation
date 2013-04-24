#define VALIDATE_PARAMETERS

using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class VoxelWorld : SimulationComponentTemplate<VoxelEntity>
	{
		public const string PARAMETER_WORLD_SIZE = "worldSize";

		private IntVector3 size;
		private byte[] voxels;

		public IntVector3 Size
		{
			get { return size; }
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

			//Create "second floor"
			for (int x = 5; x < size.x - 5; x++)
				for (int z = 5; z < size.z - 5; z++)
					SetVoxel(x, 1, z, 2);

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

		protected override void OnUpdate ()
		{
			for (int i = 0; i < entities.Count; i++)
			{
				VoxelEntity entity = entities[i];

				MoveAboveFloor(entity);

				if (entity.useGravity)
					entity.position.y -= fint.CreateFromInt(10) * SimulationTime.deltaTime;

				MoveAboveFloor(entity);
			}
		}

		private void MoveAboveFloor(VoxelEntity entity)
		{
			FVector3 position = entity.position;
			fint radius = entity.radius;
			
			FVector3 deltaBottom;
			
			if (entity.shape == VoxelEntityShape.Sphere)
				deltaBottom = FVector3.down * radius;
			else 
				deltaBottom = FVector3.down * entity.height * fint.half;
			
			FVector3 bottom = position + deltaBottom;
			
			int x = bottom.x.ToInt();
			int y = bottom.y.ToInt();
			int z = bottom.z.ToInt();
			
			if (IsValidPosition(x, y, z))
			{
				if (GetVoxel(x, y, z) != 0 && IsValidPosition(x, y + 1, z))
				{
					position.y = fint.CreateFromInt(y + 1);
					
					entity.position.y = position.y - deltaBottom.y;
				}
			}
		}
	}
}