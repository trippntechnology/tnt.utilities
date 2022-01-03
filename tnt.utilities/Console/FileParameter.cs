using System;
using System.IO;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Parameter that represents a file name
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class FileParameter : StringParameter
	{
		/// <summary>
		/// Indicates the file name must exist
		/// </summary>
		public bool MustExist { get; set; }

		/// <summary>
		/// Initializes a <see cref="FileParameter"/>
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public FileParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Gets the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <File>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Validates that value is a valid file name and that it exists if 
		/// <see cref="MustExist"/> is true. Sets value.
		/// </summary>
		/// <param name="value">Value representing a file name</param>
		public override void SetValue(object value)
		{
			try
			{
				new FileInfo(value.ToString());
			}
			catch (Exception)
			{
				throw new ArgumentException(string.Format("The '{0}' parameters expects a valid file name", this.Name));
			}

			if (MustExist && !File.Exists(value.ToString()))
			{
				throw new ArgumentException(string.Format("'{0}' does not exist", value.ToString()));
			}

			base.SetValue(value);
		}
	}
}
