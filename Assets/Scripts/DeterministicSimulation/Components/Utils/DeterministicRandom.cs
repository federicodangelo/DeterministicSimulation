using System;

namespace DeterministicSimulation
{
	public class DeterministicRandom : SimulationComponentManager
	{
		private Random rnd;

		protected override void OnInitDependencies ()
		{
			rnd = new Random(0);
		}

		public fint Value
		{
			get { return fint.CreateFromInt(rnd.Next(0, 10000)) / fint.CreateFromInt(10000); }
		}

		public fint Range(fint min, fint max)
		{
			return min + Value * (max - min);
		}

		public int Range(int min, int max)
		{
			return min + (Value * fint.CreateFromInt(max - min)).ToInt();
		}
	}
}

