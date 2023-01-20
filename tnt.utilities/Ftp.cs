using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace TNT.Utilities
{
	/// <summary>
	/// Class to handle FTP operations
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class Ftp
	{
		/// <summary>
		/// Base Uri to FTP server
		/// </summary>
		public Uri BaseUri { get; set; }

		/// <summary>
		/// User name
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Specifies whether to use binary transfer
		/// </summary>
		public bool UseBinary { get; set; }

		/// <summary>
		/// Specifies whether to use passive mode
		/// </summary>
		public bool UsePassive { get; set; }

		/// <summary>
		/// Used to specify a proxy
		/// </summary>
		public IWebProxy? Proxy { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseUri">Base Uri</param>
		/// <param name="userName">User name</param>
		/// <param name="password">Password</param>
		public Ftp(Uri baseUri, string userName, string password)
		{
			this.BaseUri = baseUri;
			this.UserName = userName;
			this.Password = password;
			this.UseBinary = true;
			this.UsePassive = true;
			this.Proxy = null;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseUri">Base Uri</param>
		public Ftp(Uri baseUri)
			: this(baseUri, string.Empty, string.Empty)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseUri">Base Uri</param>
		/// <param name="userName">User name</param>
		/// <param name="password">Password</param>
		public Ftp(string baseUri, string userName, string password)
			: this(new Uri(baseUri), userName, password)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="baseUri">Base Uri</param>
		public Ftp(string baseUri)
			: this(new Uri(baseUri))
		{
		}

		/// <summary>
		/// Uploads <paramref name="data"/> to location specified by <paramref name="relativeUri"/>
		/// </summary>
		/// <param name="data">Data to upload</param>
		/// <param name="relativeUri">Destination location relative to base Uri</param>
		/// <returns></returns>
		public string Upload(byte[] data, string relativeUri)
		{
			Uri uri = new Uri(this.BaseUri, relativeUri);

			FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(uri);
			request.Method = WebRequestMethods.Ftp.UploadFile;
			request.Proxy = this.Proxy;
			request.UseBinary = this.UseBinary;
			request.UsePassive = this.UsePassive;
			request.ContentLength = data.Length;

			if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
			{
				request.Credentials = new NetworkCredential(this.UserName, this.Password);
			}

			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}

			using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
			{
				return response.StatusDescription ?? "";
			}
		}

		/// <summary>
		/// Downloads content located at <paramref name="relativeUri"/>
		/// </summary>
		/// <param name="relativeUri">Location relative to the base Uri</param>
		/// <param name="statusDescription">Status description</param>
		/// <returns>Stream containing the data located at <paramref name="relativeUri"/></returns>
		public Stream Download(string relativeUri, out string statusDescription)
		{
			Uri uri = new Uri(this.BaseUri, relativeUri);

			// Get the object used to communicate with the server.
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
			request.Method = WebRequestMethods.Ftp.DownloadFile;
			request.Proxy = this.Proxy;
			request.UseBinary = this.UseBinary;
			request.UsePassive = this.UsePassive;

			if (!string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.Password))
			{
				request.Credentials = new NetworkCredential(this.UserName, this.Password);
			}

			FtpWebResponse response = (FtpWebResponse)request.GetResponse();
			statusDescription = response.StatusDescription ?? "";
			return response.GetResponseStream();
		}
	}
}
