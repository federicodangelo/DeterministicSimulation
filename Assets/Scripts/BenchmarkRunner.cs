using UnityEngine;
using System.Collections;
using DeterministicSimulation;

public class BenchmarkRunner : MonoBehaviour 
{
	private string benchmarkResult = "";

	void Start () 
	{
		benchmarkResult = FTestBenchmark.Benchmark(1000000);

		Debug.Log(benchmarkResult);
	}

	/*
	void OnGUI () 
	{
		GUI.Label(new Rect(10, 10, 1000, 50), benchmarkResult);
		
		if (GUI.Button(new Rect(10, 70, 100, 100), "RUN"))
			benchmarkResult = FTestBenchmark.Benchmark();
	}
	*/
}
