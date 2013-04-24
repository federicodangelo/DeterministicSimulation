using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class VoxelEntity : SimulationEntity
	{
		public const string PARAMETER_SHAPE = "shape";
		public const string PARAMETER_RADIUS = "radius";
		public const string PARAMETER_HEIGHT = "height";
		public const string PARAMETER_POSITION = "position";
		public const string PARAMETER_USE_GRAVITY = "useGravity";
		public const string PARAMETER_COLOR = "color";

		protected VoxelWorld voxelWorld;

		public VoxelEntityShape shape;
		public fint radius;
		public fint height;
		public FVector3 position;
		public bool useGravity;
		public FVector3 color;

		protected override void OnInit ()
		{
			voxelWorld = simulation.GetComponent<VoxelWorld>();

			shape = parameters.GetParameter<VoxelEntityShape>(PARAMETER_SHAPE, VoxelEntityShape.Sphere);
			radius = parameters.GetParameter<fint>(PARAMETER_RADIUS, fint.half);
			height = parameters.GetParameter<fint>(PARAMETER_HEIGHT, fint.two);
			position = parameters.GetParameter<FVector3>(PARAMETER_POSITION, FVector3.zero);
			useGravity = parameters.GetParameter<bool>(PARAMETER_USE_GRAVITY, true);
			color = parameters.GetParameter<FVector3>(PARAMETER_COLOR, FVector3.one);
		}
	}
}
