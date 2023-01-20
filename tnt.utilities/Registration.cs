using System.Diagnostics.CodeAnalysis;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace TNT.Utilities
{
	using ManagementObjects = List<Dictionary<string, string>>;
	using ManagementProperties = Dictionary<string, string>;

	/// <summary>
	/// Methods use to create and validate a registration key
	/// </summary>
	public static class Registration
	{
		private static Random m_CharacterGenerator = new Random();

		/// <summary>
		/// Validates the hash against the given seed
		/// </summary>
		/// <param name="seed">String to validate against the hash</param>
		/// <param name="hash">Hash to validate against the seed</param>
		/// <returns>True if hash is valid for the seed, false otherwise</returns>
		public static bool ValidateHash(string seed, string hash)
		{
			return hash == GenerateSHA1Hash(seed);
		}

		/// <summary>
		/// Returns the serial number for the first local fixed disk
		/// </summary>
		/// <returns>Serial number for the first local fixed disk</returns>
		[ExcludeFromCodeCoverage]
		public static string GetVolumeSerialNumber()
		{
			string result = string.Empty;
			ManagementObjects mos = GetManagementObjects("select * from win32_logicaldisk where Description = 'Local Fixed Disk' and VolumeSerialNumber is not null");

			if (mos.Count > 0)
			{
				result = mos[0]["VolumeSerialNumber"];
			}

			return result;
		}

		/// <summary>
		/// Gets the management properties for a given query
		/// </summary>
		/// <param name="query">Management object query</param>
		/// <returns>Management properties for a given query</returns>
		public static ManagementObjects GetManagementObjects(string query)
		{
			ManagementObjects result = new ManagementObjects();
			ManagementObjectSearcher mos = new ManagementObjectSearcher(query);

			foreach (ManagementObject mo in mos.Get())
			{
				ManagementProperties props = new ManagementProperties();

				foreach (PropertyData pd in mo.Properties)
				{
					if (pd.Value != null && pd.Value.ToString() != "")
					{
						props[pd.Name] = pd.Value.ToString() ?? "";
					}
				}

				result.Add(props);
			}

			return result;
		}

		/// <summary>
		/// Generates a SHA1 hash from the given seed
		/// </summary>
		/// <param name="seed">String to hash</param>
		/// <returns>SHA1 hash of seed</returns>
		public static string? GenerateSHA1Hash(string seed)
		{
			byte[] seedBytes = ASCIIEncoding.ASCII.GetBytes(seed);
			SHA1 sha1Hasher = System.Security.Cryptography.SHA1.Create();
			sha1Hasher.ComputeHash(seedBytes);
			if (sha1Hasher.Hash == null) return null;
			return Convert.ToBase64String(sha1Hasher.Hash);
		}

		/// <summary>
		/// Generates a key with the given size and segments using segment size
		/// </summary>
		/// <param name="length">Length of key to generate</param>
		/// <param name="segmentSize">Number of characters to separate with a hyphen</param>
		/// <returns>Key with the given size and segments using segment size</returns>
		public static string GenerateKey(int length, int segmentSize)
		{
			StringBuilder sb = new StringBuilder();

			for (int index = 0; index < length; index++)
			{
				int ascii = m_CharacterGenerator.Next(65, 90);
				sb.Append((char)ascii);

				if (segmentSize == 1 && index < length - 1)
				{
					sb.Append("-");
				}
				else if (segmentSize > 1 && segmentSize <= length / 2 && (index + 1) % segmentSize == 0 && index < length - 1)
				{
					sb.Append("-");
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Stores the <see cref="RegistrationKey"/> in the file specified by <paramref name="fileName"/>
		/// </summary>
		/// <param name="regKey"><see cref="RegistrationKey"/></param>
		/// <param name="fileName">Location of file</param>
		[ExcludeFromCodeCoverage]
		public static void SetRegistrationKey(RegistrationKey regKey, string fileName)
		{
			Utilities.SerializeToFile<RegistrationKey>(regKey, fileName);
		}

		/// <summary>
		/// Gets the <see cref="RegistrationKey"/> stored in <paramref name="fileName"/>
		/// </summary>
		/// <param name="fileName">Location of file</param>
		/// <returns><see cref="RegistrationKey"/></returns>
		[ExcludeFromCodeCoverage]
		public static RegistrationKey? GetRegistrationKey(string fileName)
		{
			return Utilities.DeserializeFromFile<RegistrationKey>(fileName);
		}
	}
}
