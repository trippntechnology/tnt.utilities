using System;
using System.Collections.Generic;
using System.Text;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Base for all parameters used by the Parameters class
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public abstract class Parameter
	{
		private const int USAGE_LINE_LENGTH = 70;
		private object m_Value = null;

		#region Properties
		
		/// <summary>
		/// Parameter's name
		/// </summary>
		public virtual string Name { get; internal set; }

		/// <summary>
		/// Parameter's description
		/// </summary>
		public virtual string Description { get; internal set; }

		/// <summary>
		/// Indicates if the parameter is required
		/// </summary>
		public virtual bool Required { get; internal set; }

		/// <summary>
		/// Contains the value associated with the parameter name
		/// </summary>
		public virtual object Value { get { return m_Value ?? DefaultValue; } }

		/// <summary>
		/// Default value
		/// </summary>
		public virtual object DefaultValue { get; internal set; }

		/// <summary>
		/// External <see cref="Action"/> that can be used to further validate the value.
		/// </summary>
		/// <exception cref="ArgumentException"><see cref="Action"/> should throw exception if validation fails</exception>
		public virtual Action<object> Validate { get; set; }

		#endregion

		/// <summary>
		/// Initializes the <see cref="Parameter"/>
		/// </summary>
		/// <param name="name">Name of parameter</param>
		/// <param name="description">Description of parameter</param>
		/// <param name="required">Indicates the parameter is required (default: false)</param>
		public Parameter(string name, string description, bool required)
		{
			this.Name = name;
			this.Description = description;
			this.Required = required;
		}

		/// <summary>
		/// Initializes the optional <see cref="Parameter"/> with the <paramref name="defaultValue"/>
		/// </summary>
		/// <param name="name">Name of parameter</param>
		/// <param name="description">Description of parameter</param>
		/// <param name="defaultValue">Default value</param>
		public Parameter(string name, string description, object defaultValue)
			: this(name, description, false)
		{
			this.DefaultValue = defaultValue;
		}

		/// <summary>
		/// Sets the parameter's value
		/// </summary>
		/// <param name="value">Value to set</param>
		public virtual void SetValue(object value)
		{
			if (Validate != null)
			{
				Validate(value);
			}

			if (m_Value != null)
			{
				throw new ArgumentException("Parameter was already specified.");
			}

			m_Value = value;
		}

		/// <summary>
		/// Gets the parameter's usage
		/// </summary>
		/// <returns>Parameter's usage</returns>
		public virtual string Usage()
		{
			StringBuilder usage = new StringBuilder();
			string description = string.Format("{0}{1}", Description, DefaultValue == null || string.IsNullOrEmpty(DefaultValue.ToString()) ? string.Empty : string.Format(" (default: {0})", DefaultValue.ToString()));
			string[] lines = WrapLines(description);

			for (int index = 0; index < lines.Length; index++)
			{
				usage.AppendFormat("{0}{1,-10}{2}", index == 0 ? "  /" : "\n   ", index == 0 ? Name : string.Empty, lines[index]);
			}

			return usage.ToString();
		}

		/// <summary>
		/// Override to return the syntax
		/// </summary>
		/// <returns></returns>
		public abstract string Syntax();

		/// <summary>
		/// Creates an array of <see cref="string"/> given a <paramref name="line"/> that exceeds the <see cref="USAGE_LINE_LENGTH"/>
		/// </summary>
		/// <param name="line">Line to wrap</param>
		/// <returns>Array of <see cref="string"/></returns>
		protected virtual string[] WrapLines(string line)
		{
			List<string> lines = new List<string>();

			while (line.Length > 0)
			{
				if (line.Length <= USAGE_LINE_LENGTH)
				{
					lines.Add(line);
					break;
				}
				else
				{
					string currentLine = line.Substring(0, USAGE_LINE_LENGTH);
					int lastIndexOf = currentLine.LastIndexOf(' ');

					if (lastIndexOf > 0)
					{
						currentLine = currentLine.Substring(0, lastIndexOf);
					}
					else
					{
						lastIndexOf = line.Length - 1;
					}

					lines.Add(currentLine);
					line = line.Substring(lastIndexOf + 1);
				}
			}

			return lines.ToArray();
		}
	}
}
