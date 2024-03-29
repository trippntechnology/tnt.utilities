﻿using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TNT.Utilities.CustomAttributes;

namespace TNT.Utilities
{
	/// <summary>
	/// Class that provides information about a property
	/// </summary>
	public class PropertyReflector
	{
		/// <summary>
		/// Property Info associated with the property
		/// </summary>
		protected PropertyInfo? m_PropertyInfo = null;

		/// <summary>
		/// Object that the property belongs to
		/// </summary>
		protected object? m_Object = null;

		/// <summary>
		/// Gets the display name associated with this property
		/// </summary>
		public string DisplayName
		{
			get
			{
				if (m_PropertyInfo?.GetCustomAttributes(typeof(DisplayNameAttribute), true) is DisplayNameAttribute[] attrs)
				{
					return attrs.Count() > 0 ? attrs[0].DisplayName : m_PropertyInfo.Name;
				}
				else
				{
					return "";
				}
			}
		}

		/// <summary>
		/// Gets the category associated with this property
		/// </summary>
		public string Category
		{
			get
			{
				if (m_PropertyInfo?.GetCustomAttributes(typeof(CategoryAttribute), true) is CategoryAttribute[] attrs)
				{
					return attrs.Count() > 0 ? attrs[0].Category : string.Empty;
				}
				else
				{
					return "";
				}
			}
		}

		/// <summary>
		/// Gets the value associated with this property
		/// </summary>
		public object? Value
		{
			get
			{
				return m_PropertyInfo?.GetValue(m_Object, null);
			}
		}

		/// <summary>
		/// Gets the Priority associated with this property
		/// </summary>
		public int Priority
		{
			get
			{
				if (m_PropertyInfo?.GetCustomAttributes(typeof(PropertyReflectorAttribute), true) is PropertyReflectorAttribute[] attrs)
				{
					return attrs.Count() > 0 ? attrs[0].Priority : 0;
				}
				else
				{
					return 0;
				}
			}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="obj">Object the property belongs to</param>
		/// <param name="propertyInfo">Property info associated to the property</param>
		public PropertyReflector(object? obj, PropertyInfo propertyInfo)
		{
			m_Object = obj;
			m_PropertyInfo = propertyInfo;
		}

		/// <summary>
		/// Name of the property
		/// </summary>
		/// <returns>Name of the property</returns>
		public override string ToString()
		{
			return DisplayName;
		}
	}
}
