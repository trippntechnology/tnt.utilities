using System;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// A <see cref="Parameter"/> that represents a <see cref="Version"/>
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class VersionParameter : Parameter
	{
		/// <summary>
		/// Converts base.Value to <see cref="Version"/>
		/// </summary>
		public new Version Value { get { return base.Value as Version; } }

		/// <summary>
		/// Default value constructor.
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <param name="description">Parameter description</param>
		/// <param name="defaultValue">The default value to use if one isn't provided</param>
		public VersionParameter(string name, string description, Version defaultValue)
			: this(name, description)
		{
			this.DefaultValue = defaultValue;
		}

		/// <summary>
		/// Constructor used to make the parameter required
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <param name="description">Parameter description</param>
		/// <param name="required">Set to true if required. Defaults to false.</param>
		public VersionParameter(string name, string description, bool required = false)
	: base(name, description, required)
		{
		}

		/// <summary>
		/// Returns the syntax expected by the parameter
		/// </summary>
		/// <returns>Syntax expected by the parameter</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <version>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Parses <paramref name="value"/> as a <see cref="Version"/>. If successful, sets the <see cref="Value"/>.
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a string representing a <see cref="Version"/></exception>
		/// <param name="value">An <see cref="object"/> that represents a <see cref="string"/> that represents a <see cref="Version"/></param>
		public override void SetValue(object value)
		{
			if (!Version.TryParse(value.ToString(), out Version version))
			{
				throw new ArgumentException(string.Format("Parameter '{0}' expects a string that represents a version", this.Name));
			}

			base.SetValue(version);
		}
	}
}
