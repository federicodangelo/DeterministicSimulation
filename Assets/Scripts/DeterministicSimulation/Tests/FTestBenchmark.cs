using System;
using UnityEngine;

namespace DeterministicSimulation
{
	public class FTestBenchmark
	{
		private const int ITERATIONS = 5000000;

		static public string Benchmark()
		{
			DateTime sFloat = DateTime.Now;
			BenchmarkFloat();
			DateTime eFloat = DateTime.Now;

			DateTime sFixed = DateTime.Now;
			BenchmarkFixed();
			DateTime eFixed = DateTime.Now;

			return string.Format("float: {0} fixed math: {1} slower: {2}x", (eFloat - sFloat).Ticks, (eFixed - sFixed).Ticks, (float) (((float) (eFixed - sFixed).Ticks) / ((float) (eFloat - sFloat).Ticks)) );
		}

		static public void BenchmarkFloat()
		{
			float result = 0;

			for (int i = 0; i < ITERATIONS; i++)
				result += Vector3.one.magnitude;
		}

		static public void BenchmarkFixed()
		{
			fint result = fint.zero;
			
			for (int i = 0; i < ITERATIONS; i++)
				result += FVector3.one.magnitude;
		}
	}
}

