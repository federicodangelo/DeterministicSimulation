using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class VoxelComponent : SimulationComponent
	{
		public const string PARAMETER_SHAPE = "shape";
		public const string PARAMETER_RADIUS = "radius";
		public const string PARAMETER_HEIGHT = "height";
		public const string PARAMETER_POSITION = "position";
		public const string PARAMETER_USE_GRAVITY = "useGravity";
		public const string PARAMETER_COLOR = "color";

		protected VoxelWorld voxelWorld;

		public VoxelShape shape;
		public fint radius;
		public fint height;
		public FVector3 position;
		public bool useGravity;
		public FVector3 color;

		public VoxelWorld World
		{
			get { return voxelWorld; }
		}

		protected override void OnInitDependencies ()
		{
			voxelWorld = Entity.Simulation.GetComponentManager<VoxelWorld>();
		}

		protected override void OnInit (Parameters parameters)
		{
			shape = parameters.GetParameter<VoxelShape>(PARAMETER_SHAPE, VoxelShape.Sphere);
			radius = parameters.GetParameter<fint>(PARAMETER_RADIUS, fint.half);
			height = parameters.GetParameter<fint>(PARAMETER_HEIGHT, fint.two);
			position = parameters.GetParameter<FVector3>(PARAMETER_POSITION, FVector3.zero);
			useGravity = parameters.GetParameter<bool>(PARAMETER_USE_GRAVITY, true);
			color = parameters.GetParameter<FVector3>(PARAMETER_COLOR, FVector3.one);
		}

		protected override void OnDestroy ()
		{
			voxelWorld = null;
		}
	}
}
