using System;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a flag parameter that doesn't have a value
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class FlagParameter : Parameter
	{
		/// <summary>
		/// Indicates that the flag exists. This is set when the arguments are parsed
		/// </summary>
		public virtual bool Exists { get; internal set; }

		/// <summary>
		/// Indicates to hide the syntax from the command
		/// </summary>
		public bool HideSyntax { get; set; }

		/// <summary>
		/// Returns string.Empty
		/// </summary>
		public override object Value
		{
			get { return string.Empty; }
		}

		/// <summary>
		/// Initializes the flag parameter with a name and description
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public FlagParameter(string name, string description)
			: base(name, description, false)
		{
		}

		/// <summary>
		/// Returns the syntax
		/// </summary>
		/// <returns>The syntax</returns>
		public override string Syntax()
		{
			return HideSyntax ? string.Empty : string.Format("[/{0}]", Name);
		}

		/// <summary>
		/// Validates that the <paramref name="value"/> doesn't exist and indicates that the
		/// flag exists
		/// </summary>
		/// <param name="value">Should be null or empty</param>
		public override void SetValue(object value)
		{
			if (!string.IsNullOrEmpty(value.ToString()))
			{
				throw new ArgumentException(string.Format("The '{0}' flag does not require a value", this.Name));
			}

			this.Exists = true;
		}
	}
}
