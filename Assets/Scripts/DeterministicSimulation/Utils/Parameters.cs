using System;
using System.Collections.Generic;

namespace DeterministicSimulation
{
	public class Parameters
	{
		private Dictionary<string, object> parameters = new Dictionary<string, object>();
		
		public Parameters AddParameter<T>(string id, T value)
		{
			if (parameters.ContainsKey(id))
				throw new ArgumentException("Duplicated parameter id");
			
			parameters[id] = value;

			return this;
		}
		
		public T GetParameter<T>(string id, T defaultValue)
		{
			object val;
			
			if (parameters.TryGetValue(id, out val))
			{
				if (val != null && !(val is T))
					throw new ArgumentException(string.Format("Paramater '{0}': Invalid parameter type, expected '{1}' but got '{2}'", id, typeof(T), val.GetType()));
				
				return (T) val;
			}
			
			return defaultValue;
		}

		public void Clear()
		{
			parameters.Clear();
		}
	}
}
