using UnityEngine;
using System.Collections;
using DeterministicSimulation;

public class TestRunner : MonoBehaviour 
{
	string benchmarkResult = "";



	// Use this for initialization
	void Start () 
	{
		FTestUtils.Test();

		benchmarkResult = FTestBenchmark.Benchmark();
	}
	
	// Update is called once per frame
	void OnGUI () 
	{
		GUI.Label(new Rect(10, 10, 1000, 50), benchmarkResult);

		if (GUI.Button(new Rect(10, 70, 100, 100), "RUN"))
			benchmarkResult = FTestBenchmark.Benchmark();
	}
}
