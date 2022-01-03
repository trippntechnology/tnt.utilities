using System;
using System.Linq;
using System.Net.Mail;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Parameter that represents a comma separated listing email addresses
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class EmailListParameter : StringListParameter
	{
		/// <summary>
		/// Initializes the <see cref="EmailListParameter"/>
		/// </summary>
		/// <param name="name">Parameter's name</param>
		/// <param name="description">Parameter's description</param>
		/// <param name="required">Indicates whether the parameter is required (default: false)</param>
		public EmailListParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
		}

		/// <summary>
		/// Gets the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <Email[]>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}

		/// <summary>
		/// Validates that value is a valid listing of email addresses
		/// </summary>
		/// <param name="value">Comma delimited listing of email addresses</param>
		public override void SetValue(object value)
		{
			try
			{
				string[] addresses = (from item in value.ToString().Split(',') select item.Trim()).ToArray();

				foreach (string address in addresses)
				{
					new MailAddress(address);
				}
			}
			catch
			{
				throw new ArgumentException(string.Format("The parameter '{0}' expects a comma delimited listing of email addresses", this.Name));
			}

			base.SetValue(value);
		}
	}
}
