using System;
using System.Text;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a parameter that is constrained to specific values
	/// </summary>
	/// <typeparam name="T">Enumeration</typeparam>
	[Obsolete("Use TNT.ArgumentParser")]
	public class EnumParameter<T> : Parameter
	{
		/// <summary>
		/// Returns <see cref="Value"/> as enumeration of type <typeparamref name="T"/>
		/// </summary>
		public new T Value { get { return (T)base.Value; } }

		/// <summary>
		/// Function that can be called to get the description for an enumeration value
		/// </summary>
		public Func<T,string> GetEnumDescription { get; set; }

		/// <summary>
		/// Initializes the parameter
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public EnumParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Initializes the parameter with a default value
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="defaultValue">Parameter's default value</param>
		public EnumParameter(string name, string description, T defaultValue)
			: this(name, description)
		{
			base.DefaultValue = defaultValue;
		}

		/// <summary>
		/// Gets the Usage
		/// </summary>
		/// <returns>The Usage</returns>
		public override string Usage()
		{
			StringBuilder usage = new StringBuilder();
			string description = string.Format("{0}{1}", Description, DefaultValue == null || string.IsNullOrEmpty(DefaultValue.ToString()) ? string.Empty : string.Format(" (default: {0})", DefaultValue.ToString()));

			usage.AppendLine(string.Format("  /{0,-10}{1}", Name, description));
			usage.AppendLine();
			usage.AppendLine(string.Format("{0,13}{1} values:", string.Empty, typeof(T).Name));
			usage.AppendLine();

			foreach (T t in Enum.GetValues(typeof(T)))
			{
				description = GetEnumDescription == null ? string.Empty : GetEnumDescription(t);
				usage.AppendLine(string.Format("{0,15}{1}{2}", string.Empty, t.ToString(), string.IsNullOrEmpty(description) ? description : string.Concat(" - ", description)));
			}

			return usage.ToString();

		}

		/// <summary>
		/// Gets the syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <{1}>", Name, typeof(T).Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Validates the <paramref name="value"/> against enumeration T and sets the value
		/// </summary>
		/// <param name="value">String representation of the enumerated value</param>
		/// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is not a valid T value</exception>
		public override void SetValue(object value)
		{
			try
			{
				T t = (T)Enum.Parse(typeof(T), value.ToString());
				base.SetValue(t);
			}
			catch
			{
				throw new ArgumentException(string.Format("Parameter '{0}' is not valid", this.Name));
			}
		}
	}
}
