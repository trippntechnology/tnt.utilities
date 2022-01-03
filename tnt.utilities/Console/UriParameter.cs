using System;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// A <see cref="Parameter"/> that represents a <see cref="Uri"/>
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class UriParameter : Parameter
	{
		/// <summary>
		/// Return base.Value as <see cref="Uri"/>
		/// </summary>
		public new Uri Value { get { return base.Value as Uri; } }

				/// <summary>
		/// Initializes the <see cref="Parameter"/>
		/// </summary>
		/// <param name="name">Name of parameter</param>
		/// <param name="description">Description of parameter</param>
		/// <param name="required">Indicates the parameter is required (default: false)</param>
		public UriParameter(string name, string description, bool required = false)
			:base(name, description, required)
		{
		}

		/// <summary>
		/// Initializes the optional <see cref="Parameter"/> with the <paramref name="defaultValue"/>
		/// </summary>
		/// <param name="name">Name of parameter</param>
		/// <param name="description">Description of parameter</param>
		/// <param name="defaultValue">Default value</param>
		public UriParameter(string name, string description, object defaultValue)
			: base(name, description, defaultValue)
		{
		}

		/// <summary>
		/// Returns the syntax for this parameter
		/// </summary>
		/// <returns>Syntax for this parameter</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <uri>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Overridden to validate the value is a valid Uri
		/// </summary>
		/// <param name="value">Uri</param>
		public override void SetValue(object value)
		{
			if (string.IsNullOrEmpty(value.ToString()))
			{
				throw new ArgumentException(string.Format("Parameter '{0}' expects a value", this.Name));
			}

			base.SetValue(new Uri(value.ToString()));
		}

	}
}
