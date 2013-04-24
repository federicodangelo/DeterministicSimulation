#define VALIDATE_PARAMETERS

using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class VoxelWorld 
	{
		private int sizeX;
		private int sizeY;
		private int sizeZ;
		private byte[] voxels;

		public int SizeX
		{
			get { return sizeX; }
		}

		public int SizeY
		{
			get { return sizeY; }
		}

		public int SizeZ
		{
			get { return sizeZ; }
		}

		public void Create(int sizeX, int sizeY, int sizeZ, byte defaultValue)
		{
			#if VALIDATE_PARAMETERS
			if (sizeX < 0)
				throw new System.ArgumentOutOfRangeException("sizeX", sizeX, "Invalid size");
			if (sizeY < 0)
				throw new System.ArgumentOutOfRangeException("sizeY", sizeY, "Invalid size");
			if (sizeZ < 0)
				throw new System.ArgumentOutOfRangeException("sizeZ", sizeZ, "Invalid size");
			#endif

			voxels = new byte[sizeX * sizeY * sizeZ];
			for (int i = (sizeX * sizeY * sizeZ) - 1; i >= 0; i--)
				voxels[i] = defaultValue;

			this.sizeX = sizeX;
			this.sizeY = sizeY;
			this.sizeZ = sizeZ;
		}

		public bool IsValidPosition(int x, int y, int z)
		{
			return 
				x >= 0 && x < sizeX &&
				y >= 0 && y < sizeY &&
				z >= 0 && z < sizeZ;
		}

		public byte GetVoxel(int x, int y, int z)
		{
			#if VALIDATE_PARAMETERS
			if (x < 0 || x >= sizeX)
				throw new System.ArgumentOutOfRangeException("x", x, "Invalid position");
			if (y < 0 || y >= sizeY)
				throw new System.ArgumentOutOfRangeException("y", y, "Invalid position");
			if (z < 0 || z >= sizeZ)
				throw new System.ArgumentOutOfRangeException("z", z, "Invalid position");
			#endif

			return voxels[x + y * (sizeX) + z * (sizeX * sizeY)];
		}

		public void SetVoxel(int x, int y, int z, byte voxel)
		{
			#if VALIDATE_PARAMETERS
			if (x < 0 || x >= sizeX)
				throw new System.ArgumentOutOfRangeException("x", x, "Invalid position");
			if (y < 0 || y >= sizeY)
				throw new System.ArgumentOutOfRangeException("y", y, "Invalid position");
			if (z < 0 || z >= sizeZ)
				throw new System.ArgumentOutOfRangeException("z", z, "Invalid position");
			#endif
			
			voxels[x + y * (sizeX) + z * (sizeX * sizeY)] = voxel;
		}
	}
}