using System;
using System.IO;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Parameter that represents a path
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class PathParameter : StringParameter
	{
		/// <summary>
		/// Indicates whether the path should be created if missing
		/// </summary>
		public bool CreateIfMissing { get; set; }

			/// <summary>
		/// Intializes a <see cref="PathParameter"/>
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public PathParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Initializes the path parameter with a default value
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="defaultValue">Parameter's default value</param>
		public PathParameter(string name, string description, string defaultValue)
			: this(name, description)
		{
			this.DefaultValue = defaultValue;
		}

		/// <summary>
		/// Gets the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <Path>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Validates that value is a valid path and that it exists
		/// </summary>
		/// <param name="value">Value representing a path name</param>
		public override void SetValue(object value)
		{
			try
			{
				new FileInfo(value.ToString());
			}
			catch (Exception)
			{
				throw new ArgumentException(string.Format("The '{0}' parameters expects a valid path", this.Name));
			}

			if (!Directory.Exists(value.ToString()))
			{
				if (this.CreateIfMissing)
				{
					Directory.CreateDirectory(value.ToString());
				}
				else
				{
					throw new ArgumentException(string.Format("The '{0}' parameter expects a path that exists", this.Name));
				}
			}

			base.SetValue(value);
		}
	}
}
