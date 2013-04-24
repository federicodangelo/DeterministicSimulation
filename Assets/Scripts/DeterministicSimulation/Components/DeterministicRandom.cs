using System;

namespace DeterministicSimulation
{
	public class DeterministicRandom : SimulationComponent
	{
		private Random rnd;

		protected override void OnInit ()
		{
			rnd = new Random(0);
		}

		public fint Value
		{
			get { return fint.CreateFromInt(rnd.Next(0, 10000)) / fint.CreateFromInt(10000); }
		}
	}
}

