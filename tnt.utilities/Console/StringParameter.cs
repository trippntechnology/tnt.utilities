using System;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a string parameter value
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class StringParameter : Parameter
	{
		/// <summary>
		/// Converts base.Value to <see cref="string"/>
		/// </summary>
		public new string Value { get { return base.Value == null ? string.Empty : base.Value.ToString(); } }

		/// <summary>
		/// Initializes the string parameter with a default value
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="defaultValue">Parameter's default value</param>
		public StringParameter(string name, string description, string defaultValue)
			: this(name, description)
		{
			this.DefaultValue = defaultValue;
		}

		/// <summary>
		/// Initializes the string parameter with the option of requiring a value
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public StringParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Returns the syntax for this parameter
		/// </summary>
		/// <returns>Syntax for this parameter</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <string>", Name);

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
