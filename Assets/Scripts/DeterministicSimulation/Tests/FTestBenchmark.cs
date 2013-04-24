using System;
using UnityEngine;

namespace DeterministicSimulation
{
	public class FTestBenchmark
	{
		static public string Benchmark(int iterations)
		{
			DateTime sFloat = DateTime.Now;
			BenchmarkFloat(iterations);
			DateTime eFloat = DateTime.Now;

			DateTime sFixed = DateTime.Now;
			BenchmarkFixed(iterations);
			DateTime eFixed = DateTime.Now;

			return string.Format("float: {0} ms fixed math: {1} ms slower: {2}x", (eFloat - sFloat).TotalMilliseconds, (eFixed - sFixed).TotalMilliseconds, (float) (((float) (eFixed - sFixed).Ticks) / ((float) (eFloat - sFloat).Ticks)) );
		}

		static public void BenchmarkFloat(int iterations)
		{
			float result = 0;

			for (int i = 0; i < iterations; i++)
				result += Vector3.one.magnitude;
		}

		static public void BenchmarkFixed(int iterations)
		{
			fint result = fint.zero;
			
			for (int i = 0; i < iterations; i++)
				result += FVector3.one.magnitude;
		}
	}
}

