using System;
using System.Net.Mail;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents and email address parameter
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class EMailParameter : Parameter
	{
		/// <summary>
		/// Converts base.Value to <see cref="MailAddress"/>
		/// </summary>
		public new MailAddress Value { get { return base.Value as MailAddress; } }

		/// <summary>
		/// Initializes an <see cref="EMailParameter"/>
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates parameter is required (default: false)</param>
		public EMailParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Gets the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <Email>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Verifies the <paramref name="value"/> is a valid email address and sets the value
		/// </summary>
		/// <param name="value">Email address</param>
		public override void SetValue(object value)
		{
			try
			{
				new MailAddress(value.ToString());
			}
			catch
			{
				throw new ArgumentException(string.Format("The parameter '{0}' expects an email address", this.Name));
			}

			base.SetValue(new MailAddress(value.ToString()));
		}
	}
}
