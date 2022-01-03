using System;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a parameter with double value
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class DoubleParameter : Parameter
	{
		/// <summary>
		/// Overridden to convert value to double
		/// </summary>
		public new double Value { get { return Convert.ToDouble(base.Value); } }

		/// <summary>
		/// Initializes <see cref="DoubleParameter"/>
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether parameter is required (default: false)</param>
		public DoubleParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Initializes <see cref="DoubleParameter"/> with a default value
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="defaultValue">Default parameter value</param>
		public DoubleParameter(string name, string description, double defaultValue)
			: base(name, description, defaultValue)
		{
		}

		/// <summary>
		/// Gets the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <Double>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Verifies the value is an double. Sets the value
		/// </summary>
		/// <param name="value">Double value</param>
		public override void SetValue(object value)
		{
			double result;

			if (!double.TryParse(value.ToString(), out result))
			{
				throw new ArgumentException(string.Format("The parameter '{0}' expects an double value", this.Name));
			}

			base.SetValue(result);
		}
	}
}