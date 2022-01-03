using System;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a date/time value
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class DateTimeParameter : Parameter
	{
		/// <summary>
		/// Converts base.Value to <see cref="DateTime"/>
		/// </summary>
		public new DateTime? Value { get { return (DateTime?)base.Value; } }

		/// <summary>
		/// Initializes <see cref="DateTimeParameter"/> with name and description
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <param name="description">Parameter description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public DateTimeParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Initializes <see cref="DateTimeParameter"/> with name, description, and default value
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <param name="description">Parameter description</param>
		/// <param name="defaultValue">Initializes parameter's value with default value</param>
		public DateTimeParameter(string name, string description, DateTime defaultValue)
			: base(name, description, defaultValue)
		{
		}

		/// <summary>
		/// Gets the parameter's syntax
		/// </summary>
		/// <returns>Parameter's syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <datetime>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Validates the <paramref name="value"/> is a valid <see cref="DateTime"/> and sets the parameter's value
		/// </summary>
		/// <param name="value"><see cref="DateTime"/> value</param>
		public override void SetValue(object value)
		{
			DateTime dateTime;

			// Check type
			try
			{
				dateTime = Convert.ToDateTime(value);
			}
			catch (FormatException)
			{
				throw new ArgumentException(string.Format("Invalid datetime value for parameter '{0}'", this.Name));
			}

			base.SetValue(dateTime);
		}
	}
}
