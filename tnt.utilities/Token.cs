using System;
using System.Collections.Generic;
using System.Text;

namespace TNT.Utilities
{
	/// <summary>
	/// Creates tokens
	/// </summary>
	public class Token
	{
		/// <summary>
		/// Random number generator
		/// </summary>
		protected static Random _Random = new Random(DateTime.Now.Millisecond);

		/// <summary>
		/// Characters that can be used in the token
		/// </summary>
		protected static string _Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

		/// <summary>
		/// Delimiter used by the token
		/// </summary>
		protected static string _Delimiter = "-";

		/// <summary>
		/// Create a token of <paramref name="length"/>
		/// </summary>
		/// <param name="length">Length of the token</param>
		/// <param name="customCharacters">Characters that should be used to generate a token</param>
		/// <returns>A token of <paramref name="length"/></returns>
		public static string Create(int length, char[] customCharacters = null)
		{
			StringBuilder token = new StringBuilder();
			string availableCharacters = customCharacters == null ? _Characters : string.Join(string.Empty, customCharacters);

			for (int i = 0; i < length; i++)
			{
				token.Append(availableCharacters[_Random.Next(availableCharacters.Length)]);
			}

			return token.ToString();
		}

		/// <summary>
		/// Create a token that has <paramref name="sectionCount"/> section each of length <paramref name="sectionLength"/>
		/// </summary>
		/// <param name="sectionCount">Number of sections</param>
		/// <param name="sectionLength">Length of each section</param>
		/// <param name="customCharacters">Characters that should be used to generate a token</param>
		/// <returns>A token that has <paramref name="sectionCount"/> section each of length <paramref name="sectionLength"/></returns>
		public static string Create(int sectionCount, int sectionLength, char[] customCharacters = null)
		{
			List<string> sections = new List<string>();

			for (int i = 0; i < sectionCount; i++)
			{
				sections.Add(Create(sectionLength, customCharacters));
			}

			return string.Join(_Delimiter, sections);
		}
	}
}
