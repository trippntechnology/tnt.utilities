using System;
using System.IO;
using System.Linq;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Parameter that represents a comma separated listing files
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class FileListParameter:StringListParameter
	{
		/// <summary>
		/// Initializes the <see cref="FileListParameter"/>
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public FileListParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Gets the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <File[]>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Validates that value is a valid listing of file names and that it exists
		/// </summary>
		/// <param name="value">Comma delimited listing of file name</param>
		public override void SetValue(object value)
		{
			try
			{
				string[] files = (from item in value.ToString().Split(',') select item.Trim()).ToArray(); 

				foreach (string file in files)
				{
					new FileInfo(file);

					if(!File.Exists(file))
					{
						throw new FileNotFoundException(string.Format("'{0}' does not exist", file));
					}
				}
			}
			catch (FileNotFoundException ex)
			{
				throw new ArgumentException(ex.Message);
			}
			catch (Exception)
			{
				throw new ArgumentException(string.Format("The '{0}' parameters expects a valid file name", this.Name));
			}

			base.SetValue(value);
		}
	}
}
