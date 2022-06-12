using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TNT.Utilities.CustomAttributes;

namespace TNT.Utilities
{
	/// <summary>
	/// Helps reflect the properties of a class
	/// </summary>
	/// <typeparam name="T">Object of class being reflected</typeparam>
	public class Reflector<T>
	{
		/// <summary>
		/// List of <seealso cref="PropertyReflector"/>
		/// </summary>
		public List<PropertyReflector>? Properties { get; private set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="obj">Object that should be reflected</param>
		public Reflector(T obj)
		{
			Type? type = obj?.GetType();
			if (type == null) return;
			PropertyInfo[] propInfo = type.GetProperties();
			Properties = new List<PropertyReflector>();

			foreach (PropertyInfo pi in propInfo)
			{
				PropertyReflectorAttribute[] pra = (PropertyReflectorAttribute[])pi.GetCustomAttributes(typeof(PropertyReflectorAttribute), true);

				if (pra != null && pra.Count() > 0)
				{
					Properties.Add(new PropertyReflector(obj, pi));
				}
			}
		}

		/// <summary>
		/// Returns a list of categories ordered by the highest priority property within that category
		/// </summary>
		/// <returns>List of categories ordered by the highest priority property within that category</returns>
		public List<string> GetCategoriesByPriority()
		{
			return (from p in Properties orderby p.Priority select p.Category).Distinct().ToList();
		}
	}
}
