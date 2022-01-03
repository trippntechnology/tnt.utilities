using System;
using System.Linq;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a <see cref="string"/> list parameter value
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class StringListParameter : Parameter
	{
		/// <summary>
		/// Converts <see cref="Parameter"/>.Value to <see cref="string"/>[]
		/// </summary>
		public new string[] Value
		{
			get
			{
				if (base.Value == null)
				{
					return new string[0];
				}

				return (from item in base.Value.ToString().Split(',') select item.Trim()).ToArray();
			}
		}

		/// <summary>
		/// Initializes the string list parameter with the option of making the parameter required
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public StringListParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{

		}

		/// <summary>
		/// Returns the syntax for this parameter
		/// </summary>
		/// <returns>Syntax for this parameter</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <string[]>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Overridden to validate the value is not null or empty
		/// </summary>
		/// <param name="value">String value</param>
		public override void SetValue(object value)
		{
			if (string.IsNullOrEmpty(value.ToString()))
			{
				throw new ArgumentException(string.Format("Parameter '{0}' expects a value", this.Name));
			}

			base.SetValue(value);
		}
	}
}
