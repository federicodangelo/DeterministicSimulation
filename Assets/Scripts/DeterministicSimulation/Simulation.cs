using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class Simulation 
	{
		public const string PARAMETER_STEPS_PER_SECOND = "stepsPerSecond";
		public const string PARAMETER_SIMULATION_LISTENER = "simulationListener";

		private bool initialized;

		private List<SimulationComponent> components = new List<SimulationComponent>();
		private List<SimulationEntity> entities = new List<SimulationEntity>();
		private List<SimulationEntity> entitiesToAdd = new List<SimulationEntity>();
		private List<SimulationEntity> entitiesToRemove = new List<SimulationEntity>();

		private Parameters parameters = new Parameters();

		private bool updating;
		private int nextEntityId;

		private int stepsPerSecond;
		private ISimulationListener simulationListener;

		public T AddComponent<T>() where T : SimulationComponent, new()
		{
			if (initialized)
				throw new InvalidOperationException("Can't add components after the simulation has been initialized");

			T component = new T();

			components.Add(component);

			return component;
		}

		public T GetComponent<T>() where T : SimulationComponent
		{
			for (int i = 0; i < components.Count; i++)
				if (components[i] is T)
					return (T) components[i];

			return null;
		}

		public T AddEntity<T>(Parameters parameters) where T : SimulationEntity, new()
		{
			if (!initialized)
				throw new InvalidOperationException("Can't add entities to simulations that haven't been initialized");
			
			T entity = new T();
			
			if (!updating)
			{
				entities.Add(entity);
			}
			else
			{
				entitiesToAdd.Add(entity);
			}

			entity.Init(this, parameters, nextEntityId++);

			for (int i = 0; i < components.Count; i++)
				components[i].EntityAdded(entity);

			if (simulationListener != null)
				simulationListener.EntityAdded(entity);

			return entity;
		}

		public void RemoveEntity(SimulationEntity entity)
		{
			if (!initialized)
				throw new InvalidOperationException("Can't remove entities from simulations that haven't been initialized");

			if (entity.Destroyed)
				throw new InvalidOperationException("Can't remove a destroyed entity");

			entity.Reset();

			for (int i = 0; i < components.Count; i++)
				components[i].EntityRemoved(entity);

			if (!updating)
			{
				entities.Remove(entity);
			}
			else
			{
				entitiesToAdd.Remove(entity);

				entitiesToRemove.Add(entity);
			}

			if (simulationListener != null)
				simulationListener.EntityRemoved(entity);
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

				for (int i = 0; i < components.Count; i++)
					components[i].Init(this);

				initialized = true;
			}
		}

		public void Reset()
		{
			if (initialized)
			{
				//Remove / reset entities
				while(entities.Count > 0)
					RemoveEntity(entities[entities.Count - 1]);
				nextEntityId = 0;

				//Reset components
				for (int i = 0; i < components.Count; i++)
					components[i].Reset();

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

				updating = true;

				//Updated components
				for (int i = 0; i < components.Count; i++)
					components[i].Update();

				//Update entities
				for (int i = 0; i < entities.Count; i++)
					entities[i].Update();

				if (entitiesToAdd.Count > 0)
				{
					for (int i = 0; i < entitiesToAdd.Count; i++)
						entities.Add(entitiesToAdd[i]);
					entitiesToAdd.Clear();
				}

				if (entitiesToRemove.Count > 0)
				{
					for (int i = 0; i < entitiesToRemove.Count; i++)
						entities.Remove(entitiesToRemove[i]);
					entitiesToRemove.Clear();
				}

				updating = false;

				SimulationTime.simulationTime += SimulationTime.deltaTime;
			}
		}
	}
}
