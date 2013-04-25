using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class Simulation 
	{
		public const string PARAMETER_STEPS_PER_SECOND = "stepsPerSecond";
		public const string PARAMETER_SIMULATION_LISTENER = "simulationListener";

		private bool initialized;

		private List<SimulationComponentManager> componentManagers = new List<SimulationComponentManager>();
		private List<SimulationEntity> entities = new List<SimulationEntity>();

		private Parameters parameters = new Parameters();

		private int nextEntityId;

		private int stepsPerSecond;
		private ISimulationListener simulationListener;

		public T AddComponentManager<T>() where T : SimulationComponentManager, new()
		{
			if (initialized)
				throw new InvalidOperationException("Can't add components after the simulation has been initialized");

			T component = new T();

			componentManagers.Add(component);

			return component;
		}

		public T GetComponentManager<T>() where T : SimulationComponentManager
		{
			for (int i = 0; i < componentManagers.Count; i++)
				if (componentManagers[i] is T)
					return (T) componentManagers[i];

			return null;
		}

		public SimulationEntity AddEntity(EntityTemplate template)
		{
			if (!initialized)
				throw new InvalidOperationException("Can't add entities to simulations that haven't been initialized");
			
			SimulationEntity entity = new SimulationEntity();
			
			entities.Add(entity);

			entity.Init(this, nextEntityId++, template);

			for (int i = 0; i < entity.components.Count; i++)
				for (int j = 0; j < componentManagers.Count; j++)
					componentManagers[j].ComponentAdded(entity.components[i]);

			if (simulationListener != null)
				simulationListener.EntityAdded(entity);

			return entity;
		}

		public void DestroyEntity(SimulationEntity entity)
		{
			if (!initialized)
				throw new InvalidOperationException("Can't remove entities from simulations that haven't been initialized");

			if (entity.Destroyed)
				throw new InvalidOperationException("Can't remove a destroyed entity");

			if (simulationListener != null)
				simulationListener.EntityRemoved(entity);

			for (int i = 0; i < entity.components.Count; i++)
				for (int j = 0; j < componentManagers.Count; j++)
					componentManagers[j].ComponentRemoved(entity.components[i]);

			entity.Destroy();

			entities.Remove(entity);
		}

		public void AddParameter<T>(string id, T value)
		{
			if (initialized)
				throw new InvalidOperationException("Can't add parameters after the simulation has been initialized");

			parameters.AddParameter<T>(id, value);
		}

		public T GetParameter<T>(string id, T defaultValue)
		{
			return parameters.GetParameter<T>(id, defaultValue);
		}

		public void Init()
		{
			if (!initialized)
			{
				stepsPerSecond = GetParameter<int>(PARAMETER_STEPS_PER_SECOND, 20);
				simulationListener = GetParameter<ISimulationListener>(PARAMETER_SIMULATION_LISTENER, null);

				SimulationTime.simulationTime = fint.zero;
				SimulationTime.deltaTime = fint.one / fint.CreateFromInt(stepsPerSecond);

				for (int i = 0; i < componentManagers.Count; i++)
					componentManagers[i].InitDependencies(this);

				for (int i = 0; i < componentManagers.Count; i++)
					componentManagers[i].Init();

				initialized = true;
			}
		}

		public void Reset()
		{
			if (initialized)
			{
				//Destroy entities
				while(entities.Count > 0)
					DestroyEntity(entities[entities.Count - 1]);
				nextEntityId = 0;

				//Reset components
				for (int i = 0; i < componentManagers.Count; i++)
					componentManagers[i].Reset();

				//Reset parameters
				parameters.Clear();

				accumulatedTime = fint.zero;

				initialized = false;
			}
		}

		private fint accumulatedTime;

		public void Update(fint realDeltaTime)
		{
			if (!initialized)
				return;

			accumulatedTime += realDeltaTime;

			while(initialized && accumulatedTime >= SimulationTime.deltaTime)
			{
				accumulatedTime -= SimulationTime.deltaTime;

				//Updated components
				for (int i = 0; i < componentManagers.Count; i++)
					componentManagers[i].Update();

				SimulationTime.simulationTime += SimulationTime.deltaTime;
			}
		}
	}
}
