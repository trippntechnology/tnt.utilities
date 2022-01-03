using System;
using System.Security.Cryptography.X509Certificates;

namespace TNT.Utilities.Console
{
	/// <summary>
	/// Represents a certificate
	/// </summary>
	[Obsolete("Use TNT.ArgumentParser")]
	public class CERTParameter : FileParameter
	{
		/// <summary>
		/// Returns the Value as an <see cref="X509Certificate2"/> object
		/// </summary>
		public new X509Certificate2 Value
		{
			get
			{
				X509Certificate2 cert = null;

				if (!string.IsNullOrEmpty(base.Value))
				{
					cert = new X509Certificate2(base.Value);
				}

				return cert;
			}
		}

		/// <summary>
		/// Initializes a <see cref="CERTParameter"/>
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <param name="description">Parameter description</param>
		/// <param name="required">Indicates whether parameter is required (default: false)</param>
		public CERTParameter(string name, string description, bool required = false)
			: base(name, description, required)
		{
			this.MustExist = true;
		}

		/// <summary>
		/// Returns the Syntax
		/// </summary>
		/// <returns>The Syntax</returns>
		public override string Syntax()
		{
			string syntax = string.Format("/{0} <cert>", Name);

			if (!Required)
			{
				syntax = string.Format("[{0}]", syntax);
			}

			return syntax;
		}
	}
}
