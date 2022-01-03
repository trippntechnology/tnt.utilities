using System;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a parameter with an integer value
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class IntParameter : Parameter
	{
		/// <summary>
		/// Overridden to convert value to int
		/// </summary>
		public new int Value { get { return Convert.ToInt32(base.Value); } }

		/// <summary>
		/// Initializes <see cref="IntParameter"/>
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether parameter is required (default: false)</param>
		public IntParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Initializes <see cref="IntParameter"/> with a default value
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="defaultValue">Default parameter value</param>
		public IntParameter(string name, string description, int defaultValue)
			: base(name, description, defaultValue)
		{
		}

		/// <summary>
		/// Gets the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <Int>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Verifies the value is an integer. Sets the value
		/// </summary>
		/// <param name="value">Integer value</param>
		public override void SetValue(object value)
		{
			int result;

			if (!int.TryParse(value.ToString(), out result))
			{
				throw new ArgumentException(string.Format("The parameter '{0}' expects an integer value", this.Name));
			}

			base.SetValue(result);
		}
	}
}
